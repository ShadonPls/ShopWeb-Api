using ShopWeb_Api.Models.DTO.Order;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> GetOrderByIdAsync(int id);
        Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId);
        Task<OrderResponseDTO> CreateOrderFromCartAsync(int userId);
        Task UpdateOrderStatusAsync(int orderId, string status);
    }
}
