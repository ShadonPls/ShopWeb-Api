namespace ShopWeb_Api.Models.DTO.Cart
{
    public class CartResponseDTO
    {
        public int Id { get; set; }
        public List<CartItemResponseDTO> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
    }
}
