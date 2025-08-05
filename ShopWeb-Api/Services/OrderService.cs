using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Order;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(Data.AppContext context, IMapper mapper, ICartService cartService, ILogger<OrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                _logger.LogWarning($"Заказ по данному ID {id} не найден");
                return null;
            }

            _logger.LogInformation($"Заказ по ID {id} успешно получен");
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> CreateOrderFromCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                var errorMessage = $"Заказ по данному ID {userId} не найден";
                _logger.LogWarning(errorMessage);
                return null;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                DeliveryDate = DateTime.UtcNow.AddDays(4),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList(),
                TotalPrice = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)

            };

            _context.Orders.Add(order);
            await _cartService.ClearCartAsync(cart.Id);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Заказ успешно создан");
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product).Where(o => o.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<OrderResponseDTO>>(orders);
        }

        public async Task<OperationResult> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                string errorMessage = $"Заказ с ID {orderId} не найден";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
