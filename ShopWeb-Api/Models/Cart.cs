namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Корзина покупок пользователя
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Уникальный идентификатор корзины
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя-владельца корзины
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Дата и время создания корзины
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Навигационное свойство для пользователя-владельца
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Коллекция товаров в корзине 
        /// </summary>
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
