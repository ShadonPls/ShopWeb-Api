using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Api.Models.DTO.Category;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Controllers
{
    /// <summary>
    /// Контроллер для управления категориями товаров
    /// </summary>
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Получить список всех категорий
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список категорий</returns>
        /// <response code="200">Возвращает список категорий</response>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            return Ok(categories);
        }

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        /// <param name="categoryDto">Данные новой категории</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Созданная категория</returns>
        /// <response code="201">Категория успешно создана</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="401">Неавторизованный доступ</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO categoryDto, CancellationToken cancellationToken)
        {
            var category = await _categoryService.CreateCategoryAsync(categoryDto, cancellationToken);
            return CreatedAtAction(nameof(GetAllCategories), category);
        }

        /// <summary>
        /// Обновить категорию
        /// </summary>
        /// <param name="id">ID категории</param>
        /// <param name="categoryDto">Новые данные категории</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленная категория</returns>
        /// <response code="200">Категория успешно обновлена</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="401">Неавторизованный доступ</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDTO categoryDto, CancellationToken cancellationToken)
        {
            var category = await _categoryService.UpdateCategoryAsync(id, categoryDto, cancellationToken);
            if(category == null)
                return Conflict("Неправильно введен айди категории для изменения");
            return Ok(category);
        }

        /// <summary>
        /// Удалить категорию (только для администраторов)
        /// </summary>
        /// <param name="id">ID категории</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="204">Категория успешно удалена</response>
        /// <response code="400">Невозможно удалить категорию</response>
        /// <response code="401">Неавторизованный доступ</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteCategoryAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
