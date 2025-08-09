using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Category;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Services
{
    /// <summary>
    /// Сервис для работы с категориями товаров
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        /// <summary>
        /// Конструктор сервиса категорий
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Автомаппер для преобразования DTO</param>
        /// <param name="logger">Логгер для записи событий</param>
        public CategoryService(Data.AppContext context, IMapper mapper, ILogger<CategoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получить список всех категорий
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список категорий в формате DTO</returns>
        public async Task<List<CategoryResponseDTO>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _context.Categorys.ToListAsync(cancellationToken);
            return _mapper.Map<List<CategoryResponseDTO>>(categories);
        }

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        /// <param name="categoryDto">DTO с данными для создания категории</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданная категория в формате DTO</returns>
        public async Task<CategoryResponseDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto, CancellationToken cancellationToken = default)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categorys.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Категория успешно создана");
            return _mapper.Map<CategoryResponseDTO>(category);
        }

        /// <summary>
        /// Обновить существующую категорию
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="categoryDto">DTO с новыми данными категории</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Обновленная категория в формате DTO или null, если категория не найдена</returns>
        public async Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CreateCategoryDTO categoryDto, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categorys.FindAsync(id, cancellationToken);
            if (category == null)
            {
                _logger.LogWarning($"Корзина не найдена");
                return null;
            }

            category.Name = categoryDto.Name;
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Категория успешно изменена");
            return _mapper.Map<CategoryResponseDTO>(category);
        }

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции (успех/ошибка)</returns>
        public async Task<OperationResult> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categorys
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                string errorMessage = $"Категория с ID {id} не найден";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            _context.Categorys.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Категория успешно удалена");
            return OperationResult.Success();
        }
    }
}
