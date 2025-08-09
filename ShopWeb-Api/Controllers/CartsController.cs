using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;
using ShopWeb_Api.Services.Interfaces;
using System.Security.Claims;

namespace ShopWeb_Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с корзиной пользователя
    /// </summary>
    [ApiController]
    [Route("api/carts")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> GetUserCart(CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = await _cartService.GetUserCartAsync(userId, cancellationToken);
            return Ok(cart);
        }


        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="itemDto">Данные добавляемого товара</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">Товар успешно добавлен</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="404">Товар не найден</response>
        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDTO itemDto, CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _cartService.AddItemToCartAsync(userId, itemDto, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }


        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        /// <param name="itemId">ID элемента корзины</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="204">Товар успешно удален</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Элемент корзины не найден</response>
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int itemId, CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _cartService.RemoveItemFromCartAsync(userId, itemId, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="204">Корзина успешно очищена</response>
        /// <response code="400">Ошибка при очистке</response>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _cartService.ClearCartAsync(userId, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
