using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data
{
    public class AppContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        
        public AppContext(DbContextOptions options): base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
                entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
                entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey(cp => new { cp.ProductId, cp.CategoryId });

                entity.HasOne(cp => cp.Product)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(cp => cp.ProductId);

                entity.HasOne(cp => cp.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(cp => cp.CategoryId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Login).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity.Property(u => u.RegistrationDate).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<RoleUser>(entity =>
            {
                entity.HasKey(ru => new { ru.UserId, ru.RoleId });

                entity.HasOne(ru => ru.User)
                    .WithMany(u => u.Roles)
                    .HasForeignKey(ru => ru.UserId);

                entity.HasOne(ru => ru.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(ru => ru.RoleId);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(c => c.User)
                    .WithMany(u => u.Carts)
                    .HasForeignKey(c => c.UserId);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);

                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId);

                entity.HasOne(ci => ci.Product)
                    .WithMany()
                    .HasForeignKey(ci => ci.ProductId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Status).HasMaxLength(20);
                entity.Property(o => o.OrderDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.Price).HasColumnType("decimal(18,2)");

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId);
            });
        }
    }
}
