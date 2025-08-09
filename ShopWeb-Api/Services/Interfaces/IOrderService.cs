using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Order;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken);
        Task<OrderResponseDTO> CreateOrderFromCartAsync(int userId, CancellationToken cancellationToken);
        Task<OperationResult> UpdateOrderStatusAsync(int orderId, string status, CancellationToken cancellationToken);
    }
}
