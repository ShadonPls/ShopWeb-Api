using AutoMapper;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Category;
using ShopWeb_Api.Models.DTO.Product;

namespace ShopWeb_Api.Services.Mapping
{
    /// <summary>
    /// Профиль маппинга для преобразования между сущностями Product и соответствующими DTO
    /// </summary>
    public class ProductMappingProfile : Profile
    {
        /// <summary>
        /// Конфигурирует маппинги для продукта и связанных сущностей
        /// </summary>
        public ProductMappingProfile()
        {
            // Двусторонний маппинг между Product и UpdateProductDTO
            CreateMap<Product, UpdateProductDTO>();
            CreateMap<UpdateProductDTO, Product>();

            // Двусторонний маппинг между Product и CreateProductDTO
            CreateMap<Product, CreateProductDTO>();
            CreateMap<CreateProductDTO, Product>();

            CreateMap<ProductResponseDTO, Product>();
            CreateMap<Product, ProductResponseDTO>();
        }
    }
}
