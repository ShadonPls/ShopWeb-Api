using ShopWeb_Api.Models.DTO.Cart;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDTO> GetUserCartAsync(int userId);
        Task AddItemToCartAsync(int userId, AddCartItemDTO itemDto);
        Task RemoveItemFromCartAsync(int cartItemId);
        Task ClearCartAsync(int cartId);
    }
}
