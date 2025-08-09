using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Category;

namespace ShopWeb_Api.Services.Interfaces
{
        public interface ICategoryService
        {
            Task<List<CategoryResponseDTO>> GetAllCategoriesAsync(CancellationToken cancellationToken);
            Task<CategoryResponseDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto, CancellationToken cancellationToken);
            Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CreateCategoryDTO categoryDto, CancellationToken cancellationToken);
            Task<OperationResult> DeleteCategoryAsync(int id, CancellationToken cancellationToken);
        }
}
