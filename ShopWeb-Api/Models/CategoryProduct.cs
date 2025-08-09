namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Связующая сущность между товарами и категориями
    /// </summary>
    public class CategoryProduct
    {
        /// <summary>
        /// Идентификатор связанного товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Идентификатор связанной категории
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Навигационное свойство для товара
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Навигационное свойство для категории
        /// </summary>
        public Category Category { get; set; }
    }
}
