namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Элемент корзины, представляющий товар и его количество
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Уникальный идентификатор элемента корзины
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор корзины, к которой принадлежит элемент
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Идентификатор товара в элементе корзины
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество единиц товара в корзине
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Навигационное свойство для корзины
        /// </summary>
        public Cart Cart { get; set; }

        /// <summary>
        /// Навигационное свойство для товара
        /// </summary>
        public Product Product { get; set; }
    }
}
