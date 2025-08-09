using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data.Configuration
{
    /// <summary>
    /// Настраивает модель сущности Role
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Настройка первичного ключа
            builder.HasKey(r => r.Id);

            // Настройка свойства Name
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
