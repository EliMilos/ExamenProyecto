using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Infrastructure.Identity;
using SalesInvoice.Infrastructure.Options;

namespace SalesInvoice.Infrastructure.DataContexts
{
    public class SalesInvoiceDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly string _connectionString;

        public SalesInvoiceDbContext(IOptions<DBOptions> dbOptions)
        {
            _connectionString = dbOptions.Value.ConnectionString;
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
        public DbSet<SalesOrderDetail> SalesOrderDetails => Set<SalesOrderDetail>();
        public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Muy importante llamar base para Identity

            // Configuraciones para Customer, Product, SalesOrder, SalesOrderDetail, ErrorLog como antes

            // CUSTOMER
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.Cedula).HasMaxLength(10).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasMany(e => e.SalesOrders)
                      .WithOne(o => o.Customer)
                      .HasForeignKey(o => o.CustomerId);
            });

            // PRODUCT
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.Code).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Stock).IsRequired();
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasMany(e => e.SalesOrderDetails)
                      .WithOne(d => d.Product)
                      .HasForeignKey(d => d.ProductId);
            });

            // SALES ORDER
            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.HasKey(e => e.SalesOrderId);
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Tax).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Date).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.RowVersion).IsRowVersion();
                entity.HasMany(e => e.OrderDetails)
                      .WithOne(d => d.SalesOrder)
                      .HasForeignKey(d => d.SalesOrderId);
            });

            // SALES ORDER DETAIL
            modelBuilder.Entity<SalesOrderDetail>(entity =>
            {
                entity.HasKey(e => e.SalesOrderDetailId);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Tax).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
            });

            // ERROR LOG
            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => e.ErrorLogId);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Detail);
                entity.Property(e => e.Controller).HasMaxLength(100);
                entity.Property(e => e.Method).HasMaxLength(100);
                entity.Property(e => e.UserEmail).HasMaxLength(100);
                entity.Property(e => e.LoggedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("GETDATE()");
            });

            // Opcional: configuración adicional para ApplicationUser y ApplicationRole si quieres
        }
    }
}
