namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Представляет пользователя системы
    /// </summary>
    public class User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин пользователя для входа в систему
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона пользователя (необязательное поле)
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Адрес пользователя (необязательное поле)
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Дата и время регистрации пользователя в системе
        /// </summary>
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Коллекция заказов, связанных с данным пользователем
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Коллекция корзин, связанных с данным пользователем
        /// </summary>
        public ICollection<Cart> Carts { get; set; }

        /// <summary>
        /// Коллекция ролей, назначенных данному пользователю
        /// </summary>
        public ICollection<RoleUser> Roles { get; set; }
    }
}
