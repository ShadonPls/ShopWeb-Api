using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDTO> GetUserCartAsync(int userId, CancellationToken cancellationToken);
        Task<OperationResult> AddItemToCartAsync(int userId, AddCartItemDTO itemDto, CancellationToken cancellationToken);
        Task<OperationResult> RemoveItemFromCartAsync(int userId, int cartItemId, CancellationToken cancellationToken);
        Task<OperationResult> ClearCartAsync(int userId, CancellationToken cancellationToken);
    }
}
