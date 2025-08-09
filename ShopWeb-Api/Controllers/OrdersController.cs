using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Order;
using ShopWeb_Api.Services.Interfaces;
using System.Security.Claims;

namespace ShopWeb_Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с заказами
    /// </summary>
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


        /// <summary>
        /// Создать новый заказ из корзины пользователя
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Созданный заказ</returns>
        /// <response code="200">Заказ успешно создан</response>
        /// <response code="400">Ошибка при создании заказа</response>
        /// <response code="409">Корзина пуста или не найдена</response>
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = await _orderService.CreateOrderFromCartAsync(userId, cancellationToken);
            if(order== null)
                return Conflict($"Корзина пуста");
            return Ok(order);
        }

        /// <summary>
        /// Получить список заказов текущего пользователя
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список заказов пользователя</returns>
        /// <response code="200">Возвращает список заказов</response>
        [HttpGet]
        public async Task<IActionResult> GetUserOrders(CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetUserOrdersAsync(userId, cancellationToken);
            return Ok(orders);
        }


        /// <summary>
        /// Получить заказ по ID
        /// </summary>
        /// <param name="id">ID заказа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Детали заказа</returns>
        /// <response code="200">Возвращает детали заказа</response>
        /// <response code="401">Неавторизованный доступ</response>
        /// <response code="404">Заказ не найден</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if(orders == null)
                return Conflict($"Не найден продукт по введенному Id {id}");
            return Ok(orders);
        }

        /// <summary>
        /// Обновить статус заказа
        /// </summary>
        /// <param name="orderId">ID заказа</param>
        /// <param name="statusDto">Новый статус</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="204">Статус успешно обновлен</response>
        /// <response code="400">Некорректный статус</response>
        /// <response code="401">Неавторизованный доступ</response>
        /// <response code="404">Заказ не найден</response>
        [HttpPatch("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO statusDto, CancellationToken cancellationToken)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, statusDto.Status, cancellationToken);
            return NoContent();
        }
    }

}
