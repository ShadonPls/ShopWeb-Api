using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data.Configuration
{
    /// <summary>
    /// Настраивает модель связующей сущности RoleUser
    /// </summary>
    public class RoleUserConfiguration : IEntityTypeConfiguration<RoleUser>
    {
        public void Configure(EntityTypeBuilder<RoleUser> builder)
        {
            // Установка составного первичного ключа
            builder.HasKey(ru => new { ru.UserId, ru.RoleId });

            // Настройка связи с User
            builder.HasOne(ru => ru.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ru => ru.UserId);

            // Настройка связи с Role
            builder.HasOne(ru => ru.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(ru => ru.RoleId);
        }
    }
}
