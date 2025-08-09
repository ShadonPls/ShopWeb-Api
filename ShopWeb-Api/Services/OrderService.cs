using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Order;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Services
{
    /// <summary>
    /// Сервис для работы с заказами
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderService> _logger;

        /// <summary>
        /// Конструктор сервиса заказов
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Автомаппер для преобразования DTO</param>
        /// <param name="cartService">Сервис работы с корзиной</param>
        /// <param name="logger">Логгер для записи событий</param>
        public OrderService(Data.AppContext context, IMapper mapper, ICartService cartService, ILogger<OrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Получить заказ по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>DTO заказа или null, если заказ не найден</returns>
        public async Task<OrderResponseDTO> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            if (order == null)
            {
                _logger.LogWarning($"Заказ по данному ID {id} не найден");
                return null;
            }

            _logger.LogInformation($"Заказ по ID {id} успешно получен");
            return _mapper.Map<OrderResponseDTO>(order);
        }

        /// <summary>
        /// Создать заказ из корзины пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>DTO созданного заказа или null, если корзина пуста</returns>
        public async Task<OrderResponseDTO> CreateOrderFromCartAsync(int userId, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

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
            await _cartService.ClearCartAsync(cart.Id, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Заказ успешно создан");
            return _mapper.Map<OrderResponseDTO>(order);
        }

        /// <summary>
        /// Получить список заказов пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список DTO заказов пользователя</returns>
        public async Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product).Where(o => o.UserId == userId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<OrderResponseDTO>>(orders);
        }

        /// <summary>
        /// Обновить статус заказа
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="status">Новый статус заказа</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции (успех/ошибка)</returns>
        public async Task<OperationResult> UpdateOrderStatusAsync(int orderId, string status, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FindAsync(orderId, cancellationToken);
            if (order == null)
            {
                string errorMessage = $"Заказ с ID {orderId} не найден";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            order.Status = status;
            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
