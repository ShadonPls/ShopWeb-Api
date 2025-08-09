using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data.Configuration
{
    /// <summary>
    /// Настраивает модель связующей сущности CategoryProduct
    /// </summary>
    public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            // Установка составного первичного ключа
            builder.HasKey(cp => new { cp.ProductId, cp.CategoryId });

            // Настройка связи с Product
            builder.HasOne(cp => cp.Product)
                .WithMany(p => p.Categories)
                .HasForeignKey(cp => cp.ProductId);

            // Настройка связи с Category
            builder.HasOne(cp => cp.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(cp => cp.CategoryId);
        }
    }
}
