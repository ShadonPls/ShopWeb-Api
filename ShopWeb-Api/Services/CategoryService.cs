using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Category;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(Data.AppContext context, IMapper mapper, ILogger<CategoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categorys.ToListAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categorys.Add(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Категория успешно создана");
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, CreateCategoryDTO categoryDto)
        {
            var category = await _context.Categorys.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Корзина не найдена");
                return null;
            }

            category.Name = categoryDto.Name;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Категория успешно изменена");
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<OperationResult> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categorys
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                string errorMessage = $"Категория с ID {id} не найден";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            _context.Categorys.Remove(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Категория успешно удалена");
            return OperationResult.Success();
        }
    }
}
