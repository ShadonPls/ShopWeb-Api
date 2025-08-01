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

        public OrderService(Data.AppContext context, IMapper mapper, ICartService cartService)
        {
            _context = context;
            _mapper = mapper;
            _cartService = cartService;
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> CreateOrderFromCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty");

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

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            order.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
