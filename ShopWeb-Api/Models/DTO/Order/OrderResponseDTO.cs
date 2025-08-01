namespace ShopWeb_Api.Models.DTO.Order
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
