using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Api.Models.DTO.Product;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с товарами
    /// </summary>
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Получить список всех товаров
        /// </summary>
        /// <param name="filter">Параметры фильтрации и пагинации</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список товаров</returns>
        /// <response code="200">Возвращает список товаров</response>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProductsAsync(cancellationToken);
            return Ok(products);
        }

        /// <summary>
        /// Получить товар по ID
        /// </summary>
        /// <param name="id">ID товара</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Данные товара</returns>
        /// <response code="200">Возвращает данные товара</response>
        /// <response code="404">Товар не найден</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            if(product== null)
                return Conflict($"Не найден продукт по введенному Id {id}");
            return Ok(product);
        }

        /// <summary>
        /// Создать новый товар
        /// </summary>
        /// <param name="productDto">Данные нового товара</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Созданный товар</returns>
        /// <response code="201">Товар успешно создан</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="401">Неавторизованный доступ</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDto, CancellationToken cancellationToken)
        {
            var product = await _productService.CreateProductAsync(productDto, cancellationToken);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Обновить товар 
        /// </summary>
        /// <param name="id">ID товара</param>
        /// <param name="productDto">Новые данные товара</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="204">Товар успешно обновлен</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="401">Неавторизованный доступ</response>
        /// <response code="404">Товар не найден</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto, CancellationToken cancellationToken)
        {
            await _productService.UpdateProductAsync(id, productDto, cancellationToken);
            return NoContent();
        }


        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id">ID товара</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="204">Товар успешно удален</response>
        /// <response code="400">Невозможно удалить товар</response>
        /// <response code="401">Неавторизованный доступ</response>
        /// <response code="404">Товар не найден</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
