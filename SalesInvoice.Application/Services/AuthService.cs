using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SalesInvoice.Application.DTOs.Auth;
using SalesInvoice.Infrastructure.Identity;
using System.Security.Claims;
using System.Text;

namespace SalesInvoice.Application.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
                return (false, "El correo ya está registrado.");

            var existingUserByCedula = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Cedula == dto.Cedula);

            if (existingUserByCedula != null)
                return (false, "La cédula ya está registrada.");

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                Cedula = dto.Cedula
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, string.Join(" ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Usuario");

            return (true, "Registro exitoso.");
        }




        // Nuevo LoginAsync que genera JWT en lugar de SignInResult
        public async Task<(bool Success, string Message, string Token)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return (false, "Usuario no encontrado", null);

            if (await _userManager.IsLockedOutAsync(user))
                return (false, "Usuario bloqueado. Por favor contacte al administrador.", null);

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (await _userManager.IsLockedOutAsync(user))
                    return (false, "Usuario bloqueado tras varios intentos fallidos. Contacte al administrador.", null);

                return (false, "Credenciales incorrectas. Intente de nuevo.", null);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear SecurityTokenDescriptor para JsonWebTokenHandler
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JsonWebTokenHandler();
            string token = tokenHandler.CreateToken(tokenDescriptor);

            return (true, "Login exitoso", token);
        }



        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(bool Success, string Message)> AssignRoleAsync(string userEmail, string role)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return (false, "Usuario no encontrado");

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Quitar todos los roles actuales
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return (false, "Error al quitar roles previos");

            // Asignar el nuevo rol
            var addResult = await _userManager.AddToRoleAsync(user, role);
            if (!addResult.Succeeded)
                return (false, $"Error al asignar rol '{role}'");

            // Ajustar LockoutEnabled según el rol
            if (role == "Admin")
            {
                user.LockoutEnabled = false;
            }
            else if (role == "Usuario")
            {
                user.LockoutEnabled = true;
            }

            await _userManager.UpdateAsync(user);

            return (true, $"Rol '{role}' asignado y LockoutEnabled ajustado para el usuario '{userEmail}'");
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<(ApplicationUser? User, IList<string> Roles)> GetUserAndRolesByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (null, new List<string>());

            var roles = await _userManager.GetRolesAsync(user);
            return (user, roles);
        }


        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .Where(u => u.Email.ToLower() != "admin@gmail.com")
                .ToListAsync();
        }

        public async Task<List<object>> GetAllUsersWithRolesAsync()
        {
            var users = _userManager.Users
                .Where(u => u.Email.ToLower() != "admin@gmail.com")
                .ToList();

            var result = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Sin Rol";

                result.Add(new
                {
                    user.Id,
                    user.Email,
                    user.Cedula,
                    user.UserName,
                    IsBlocked = user.LockoutEnd.HasValue,
                    user.AccessFailedCount,
                    Role = role
                });
            }

            return result;
        }


        public async Task<(bool Success, string Message, string ConcurrencyStamp)> UpdateUserAsync(
    string userId,
    string email,
    string cedula,
    string concurrencyStamp,
    string role,
    string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Usuario no encontrado.", null);

            if (user.ConcurrencyStamp != concurrencyStamp)
                return (false, "Los datos fueron modificados por otro administrador. Actualice la información y vuelva a intentar.", null);

            var existingUserByEmail = await _userManager.FindByEmailAsync(email);
            if (existingUserByEmail != null && existingUserByEmail.Id != userId)
                return (false, "El correo ya está en uso por otro usuario.", null);

            var existingUserByCedula = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Cedula == cedula && u.Id != userId);

            if (existingUserByCedula != null)
                return (false, "La cédula ya está registrada en otro usuario.", null);

            user.Email = email;
            user.UserName = email;
            user.Cedula = cedula;

            // Cambiar rol si es necesario
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, role);
            }

            // Ajustar bloqueo según rol
            if (role == "Admin")
            {
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
            }
            else
            {
                user.LockoutEnabled = true;
            }

            // Validar que la nueva contraseña no sea igual a la anterior
            if (!string.IsNullOrEmpty(newPassword))
            {
                var passwordValid = await _userManager.CheckPasswordAsync(user, newPassword);
                if (passwordValid)
                    return (false, "La nueva contraseña no puede ser igual a la anterior.", null);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!passwordResult.Succeeded)
                    return (false, "Error al cambiar la contraseña.", null);
            }

            // Guardar cambios generales
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return (false, string.Join(" ", updateResult.Errors.Select(e => e.Description)), null);

            // Si se especificó una nueva contraseña, resetearla
            if (!string.IsNullOrEmpty(newPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!passwordResult.Succeeded)
                    return (false, "Error al cambiar la contraseña.", null);
            }

            return (true, "Usuario actualizado correctamente.", user.ConcurrencyStamp);
        }






        public async Task<(bool Success, string Message)> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Usuario no encontrado.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return (false, string.Join(" ", result.Errors.Select(e => e.Description)));

            return (true, "Usuario eliminado correctamente.");
        }

        public async Task<(bool Success, string Message)> ToggleLockoutAsync(string userId, bool lockUser)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Usuario no encontrado.");

            if (lockUser)
            {
                // Bloquear indefinidamente (hasta fecha lejana)
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                return (true, "Usuario bloqueado correctamente.");
            }
            else
            {
                // Desbloquear
                await _userManager.SetLockoutEndDateAsync(user, null);
                await _userManager.ResetAccessFailedCountAsync(user);
                return (true, "Usuario desbloqueado correctamente.");
            }
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<(bool Success, string Message)> RegisterWithRoleAsync(RegisterWithRoleDto dto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
                return (false, "El correo ya está registrado.");

            var existingUserByCedula = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Cedula == dto.Cedula);

            if (existingUserByCedula != null)
                return (false, "La cédula ya está registrada.");

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                Cedula = dto.Cedula
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, string.Join(" ", result.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);

            if (!roleResult.Succeeded)
                return (false, "Error al asignar el rol.");

            // Ajustar LockoutEnabled según rol
            if (dto.Role == "Admin")
            {
                user.LockoutEnabled = false;
            }
            else
            {
                user.LockoutEnabled = true;
            }

            await _userManager.UpdateAsync(user);

            return (true, "Usuario registrado exitosamente.");
        }

    }
}
