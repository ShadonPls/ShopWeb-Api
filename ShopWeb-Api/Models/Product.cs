namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Представляет товар в системе
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Уникальный идентификатор товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание товара
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Цена товара в валюте системы
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Количество единиц товара на складе
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Дата и время создания записи о товаре
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Коллекция элементов корзины, связанных с данным товаром
        /// </summary>
        public ICollection<CartItem> CartItems { get; set; }

        /// <summary>
        /// Коллекция категорий, к которым принадлежит товар
        /// </summary>
        public ICollection<CategoryProduct> Categories { get; set; } = new List<CategoryProduct>();

        /// <summary>
        /// Коллекция элементов заказа, содержащих данный товар
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
