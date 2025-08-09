namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Категория товаров в системе
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Уникальный идентификатор категории
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название категории (например: "Электроника", "Одежда")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Коллекция товаров в данной категории
        /// </summary>
        public ICollection<CategoryProduct> Products { get; set; }
    }
}
