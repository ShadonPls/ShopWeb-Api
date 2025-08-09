using ShopWeb_Api.Models.DTO.Category;

namespace ShopWeb_Api.Models.DTO.Product
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CategoryResponseDTO> Categories { get; set; } = new();
    }
}
