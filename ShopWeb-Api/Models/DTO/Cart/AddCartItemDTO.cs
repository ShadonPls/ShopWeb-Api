using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Api.Models.DTO.Cart
{
    public class AddCartItemDTO
    {
        [Required(ErrorMessage = "Укажите ID товара")]
        public int ProductId { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Минимум 1 товар")]
        public int Quantity { get; set; } = 1;
    }
}
