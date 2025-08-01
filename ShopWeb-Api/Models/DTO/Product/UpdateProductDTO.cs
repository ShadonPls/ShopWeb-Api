using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Api.Models.DTO.Product
{
    public class UpdateProductDTO
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue)]
        public int? Stock { get; set; }

        public List<int>? CategoryIds { get; set; }
    }
}
