namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Представляет единицу товара в заказе
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Уникальный идентификатор позиции заказа
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор связанного товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Идентификатор заказа, к которому относится позиция
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Количество единиц товара в заказе
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Цена товара на момент оформления заказа
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Навигационное свойство для связанного товара
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Навигационное свойство для связанного заказа
        /// </summary>
        public Order Order { get; set; }
    }
}
