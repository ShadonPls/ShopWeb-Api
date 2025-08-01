using ShopWeb_Api.Models.DTO.Category;

namespace ShopWeb_Api.Services.Interfaces
{
        public interface ICategoryService
        {
            Task<List<CategoryDTO>> GetAllCategoriesAsync();
            Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto);
            Task<CategoryDTO> UpdateCategoryAsync(int id, CreateCategoryDTO categoryDto);
            Task DeleteCategoryAsync(int id);
        }
}
