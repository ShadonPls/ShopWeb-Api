using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Order;
using ShopWeb_Api.Services.Interfaces;
using System.Security.Claims;

namespace ShopWeb_Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = await _orderService.CreateOrderFromCartAsync(userId);
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var orders = await _orderService.GetOrderByIdAsync(id);
            if(orders == null)
                return Conflict($"Не найден продукт по введенному Id {id}");
            return Ok(orders);
        }

        [HttpPatch("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO statusDto)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, statusDto.Status);
            return NoContent();
        }
    }

}
