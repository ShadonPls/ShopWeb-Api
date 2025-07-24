namespace ShopWeb_Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public decimal TotalPrice { get; set; }
        public string Status {  get; set; }
        public DateTime OrderDate {  get; set; }
        public DateTime DeliveryDate { get; set; }
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
