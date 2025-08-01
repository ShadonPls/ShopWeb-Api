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

        public CategoryService(Data.AppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, CreateCategoryDTO categoryDto)
        {
            var category = await _context.Categorys.FindAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            category.Name = categoryDto.Name;
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categorys
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new KeyNotFoundException("Category not found");

            if (category.Products.Any())
                throw new InvalidOperationException("Cannot delete category with linked products");

            _context.Categorys.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
