using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Data.Configuration;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data
{
    /// <summary>
    /// Контекст базы данных приложения
    /// </summary>
    /// <remarks>
    /// Предоставляет доступ к таблицам базы данных через DbSet-свойства.
    /// Настраивает отношения между сущностями и применяет конфигурации.
    /// </remarks>
    public class AppContext : DbContext
    {
        /// <summary>
        /// Таблица пользователей системы
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица ролей системы
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Таблица товаров
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Таблица заказов
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Таблица позиций заказов
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Таблица категорий товаров
        /// </summary>
        public DbSet<Category> Categorys { get; set; }

        /// <summary>
        /// Таблица корзин пользователей
        /// </summary>
        public DbSet<Cart> Carts { get; set; }

        /// <summary>
        /// Таблица элементов корзин
        /// </summary>
        public DbSet<CartItem> CartItems { get; set; }

        /// <summary>
        /// Таблица связи пользователей и ролей
        /// </summary>
        public DbSet<RoleUser> RoleUsers { get; set; }

        /// <summary>
        /// Таблица связи товаров и категорий
        /// </summary>
        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        /// <summary>
        /// Конструктор контекста
        /// </summary>
        /// <param name="options">Настройки контекста</param>
        public AppContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Настройка модели данных
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Применение индивидуальных конфигураций сущностей
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryProductConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleUserConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        }
    }
}
