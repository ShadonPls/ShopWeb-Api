namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Представляет заказ в системе
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Уникальный идентификатор заказа
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, оформившего заказ
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Общая стоимость заказа
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Текущий статус заказа (например: "В обработке", "Доставлен")
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Дата и время оформления заказа
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Планируемая дата доставки заказа
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Навигационное свойство для пользователя, оформившего заказ
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Коллекция позиций заказа
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
