using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data.Configuration
{
    /// <summary>
    /// Настраивает модель сущности Cart
    /// </summary>
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Установка первичного ключа
            builder.HasKey(c => c.Id);

            // Настройка свойства CreatedAt
            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Настройка связи с пользователем
            builder.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId);
        }
    }
}
