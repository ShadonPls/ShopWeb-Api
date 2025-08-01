using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Api.Models.DTO.Product
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Название обязательно")]
        [MaxLength(100, ErrorMessage = "Максимум 100 символов")]
        public string Name { get; set; }

        
        [MaxLength(500, ErrorMessage = "Максимум 500 символов")]
        public string Description { get; set; }

        
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть положительной")]
        public decimal Price { get; set; }

        
        [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int Stock { get; set; }

        
        [Required(ErrorMessage = "Укажите хотя бы одну категорию")]
        public List<int> CategoryIds { get; set; } = new();
    }
}
