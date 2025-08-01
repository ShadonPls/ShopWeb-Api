using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Api.Models.DTO.Order
{
    public class UpdateOrderStatusDTO
    {
        [Required(ErrorMessage = "Укажите статус")]
        public string Status { get; set; } 
    }
}
