using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Product;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO> GetProductByIdAsync(int id);
        Task<List<ProductResponseDTO>> GetAllProductsAsync();
        Task<ProductResponseDTO> CreateProductAsync(CreateProductDTO productDto);
        Task<OperationResult> UpdateProductAsync(int id, UpdateProductDTO productDto);
        Task<OperationResult> DeleteProductAsync(int id);
    }
}
