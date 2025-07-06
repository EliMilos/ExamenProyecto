using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesInvoice.API.Filters;
using SalesInvoice.API.Middleware;
using SalesInvoice.Application.Validators.Auth;
using SalesInvoice.Infrastructure.DataContexts;
using SalesInvoice.Infrastructure.Identity;
using SalesInvoice.Infrastructure.Options;
using SalesInvoice.IOC;
using System.Text;
using System.Text.Json;

//Commit Prueba exitosa
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SalesInvoice API", Version = "v1" });

    // Agregar autenticación JWT a Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Autorización JWT usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Identity services with custom user and role
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // Bloqueo indefinido (solo admin puede desbloquear)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(365 * 100); // 100 años
})
.AddEntityFrameworkStores<SalesInvoiceDbContext>()
.AddDefaultTokenProviders();

// Leer la clave secreta del JWT desde la configuración
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];

// Configurar autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                context.Token = token;
            }
            else
            {
                context.Token = null;
            }
            return Task.CompletedTask;
        },

        OnAuthenticationFailed = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "Token inválido o expirado",
                details = context.Exception?.Message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        },

        OnChallenge = async context =>
        {
            // Este evento se ejecuta cuando no hay token o es inválido
            context.HandleResponse(); // Evita el comportamiento por defecto

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "No autorizado. Token requerido o inválido"
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        },

        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "Token válido pero privilegios insuficientes"
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    };
});

builder.Services.Configure<DBOptions>(builder.Configuration.GetSection(DBOptions.SectionKey));

builder.Services.AddDbContext<SalesInvoiceDbContext>((serviceProvider, options) =>
{
    var dbOptions = serviceProvider.GetRequiredService<IOptions<DBOptions>>();
    options.UseSqlServer(dbOptions.Value.ConnectionString);
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationExceptionFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();


builder.Services.AddSalesInvoiceServices(
            dbOptions => builder.Configuration.GetSection(DBOptions.SectionKey).Bind(dbOptions)
        );

builder.Services.AddAuthorization();

// Configurar CORS para permitir cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

//app.UseMiddleware<ErrorLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
