using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Api.Models.DTO.User
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [MaxLength(50, ErrorMessage = "Максимум 50 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string? Email { get; set; }
    }
}
