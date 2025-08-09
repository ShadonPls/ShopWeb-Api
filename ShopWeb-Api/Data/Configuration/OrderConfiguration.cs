using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data.Configuration
{
    /// <summary>
    /// Настраивает модель сущности Order
    /// </summary>
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Настройка первичного ключа
            builder.HasKey(o => o.Id);

            // Настройка свойства TotalPrice
            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            // Настройка свойства Status
            builder.Property(o => o.Status)
                .HasMaxLength(20);

            // Настройка временных меток
            builder.Property(o => o.OrderDate)
                .HasDefaultValueSql("GETDATE()");

            // Настройка связи с пользователем
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
        }
    }
}
