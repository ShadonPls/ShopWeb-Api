namespace ShopWeb_Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CategoryProduct> Products { get; set; }
    }
}
