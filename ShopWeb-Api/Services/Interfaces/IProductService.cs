using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Product;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO> GetProductByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<ProductResponseDTO>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task<ProductResponseDTO> CreateProductAsync(CreateProductDTO productDto, CancellationToken cancellationToken);
        Task<OperationResult> UpdateProductAsync(int id, UpdateProductDTO productDto, CancellationToken cancellationToken);
        Task<OperationResult> DeleteProductAsync(int id, CancellationToken cancellationToken);
    }
}
