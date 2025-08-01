using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Api.Models.DTO.Category
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Название обязательно")]
        [MaxLength(50, ErrorMessage = "Максимум 50 символов")]
        public string Name { get; set; }
    }
}
