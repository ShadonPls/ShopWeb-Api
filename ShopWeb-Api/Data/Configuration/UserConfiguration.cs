using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopWeb_Api.Models;

namespace ShopWeb_Api.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Настройка первичного ключа
            builder.HasKey(u => u.Id);

            // Настройка свойства Login
            builder.Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(50);

            // Настройка свойства Password
            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            // Настройка временных меток
            builder.Property(u => u.RegistrationDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
