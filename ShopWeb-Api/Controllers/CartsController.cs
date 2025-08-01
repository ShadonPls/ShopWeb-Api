using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;
using ShopWeb_Api.Services.Interfaces;
using System.Security.Claims;

namespace ShopWeb_Api.Controllers
{
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
        public async Task<IActionResult> GetUserCart()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDTO itemDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cartService.AddItemToCartAsync(userId, itemDto);
            return Ok();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int itemId)
        {
            await _cartService.RemoveItemFromCartAsync(itemId);
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }
}
