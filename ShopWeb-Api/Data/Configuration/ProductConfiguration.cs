using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data.Configuration
{
    /// <summary>
    /// Настраивает модель сущности Product
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Настройка первичного ключа
            builder.HasKey(x => x.Id);

            // Настройка свойства Name
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Настройка свойства Price
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            // Настройка временных меток
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
