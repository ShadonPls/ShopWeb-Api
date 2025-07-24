namespace ShopWeb_Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt {  get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<CategoryProduct> Categories { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
