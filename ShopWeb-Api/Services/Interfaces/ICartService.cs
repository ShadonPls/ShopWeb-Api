using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDTO> GetUserCartAsync(int userId);
        Task<OperationResult> AddItemToCartAsync(int userId, AddCartItemDTO itemDto);
        Task<OperationResult> RemoveItemFromCartAsync(int userId, int cartItemId);
        Task<OperationResult> ClearCartAsync(int userId);
    }
}
