using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Api.Models.DTO.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Логин обязателен")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
    }
}
