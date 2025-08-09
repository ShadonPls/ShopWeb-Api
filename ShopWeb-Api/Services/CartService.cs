using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Services
{
    /// <summary>
    /// Сервис для работы с корзиной пользователя
    /// </summary>
    public class CartService : ICartService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        /// <summary>
        /// Конструктор сервиса корзины
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Автомаппер для преобразования DTO</param>
        /// <param name="logger">Логгер для записи событий</param>
        public CartService(Data.AppContext context, IMapper mapper, ILogger<CartService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получить корзину пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO корзины пользователя</returns>
        public async Task<CartResponseDTO> GetUserCartAsync(int userId, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Корзина создана");
            }

            return _mapper.Map<CartResponseDTO>(cart);
        }

        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="itemDto">DTO добавляемого товара</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Результат операции</returns>
        public async Task<OperationResult> AddItemToCartAsync(int userId, AddCartItemDTO itemDto, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken)
                ?? new Cart { UserId = userId };

            var product = await _context.Products.FindAsync(itemDto.ProductId, cancellationToken);

            if (product == null)
            {
                string errorMessage = $"Товар с ID {itemDto.ProductId} не найден";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            if (product.Stock < itemDto.Quantity)
            {
                string errorMessage = $"Недостаточный остаток товара {product.Name}. Доступно: {product.Stock}, запрошено: {itemDto.Quantity}";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Товар {product.Name} успешно добавлен в корзину");
            return OperationResult.Success();
        }

        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="cartItemId">ID элемента корзины</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Результат операции</returns>
        public async Task<OperationResult> RemoveItemFromCartAsync(int userId, int cartItemId,  CancellationToken cancellationToken = default)
        {
            var item = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId, cancellationToken);
            if (item == null)
            {
                var errorMessage = $"Элемент корзины {cartItemId} не найден или не принадлежит пользователю";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Элемент корзины {cartItemId} успешно удален");
            return OperationResult.Success();
        }

        /// <summary>
        /// Очистить корзину пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Результат операции</returns>
        public async Task<OperationResult> ClearCartAsync(int userId, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                var errorMessage = $"Корзина не найдена";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            if (!cart.CartItems.Any())
            {
                var errorMessage = $"Корзина уже пуста";
                _logger.LogWarning(errorMessage);
                return OperationResult.Success(errorMessage);
            }

            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Корзина очищена");
            return OperationResult.Success();
        }
    }
}
