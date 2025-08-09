using AutoMapper;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Category;

namespace ShopWeb_Api.Services.Mapping
{
    /// <summary>
    /// Профиль маппинга для преобразования между сущностями Category и соответствующими DTO
    /// </summary>
    public class CategoryMappingProfile : Profile
    {
        /// <summary>
        /// Конфигурирует маппинги для категорий и связанных сущностей
        /// </summary>
        public CategoryMappingProfile()
        {
            // Прямой маппинг Category в CategoryResponseDTO
            CreateMap<Category, CategoryResponseDTO>();

            // Двусторонний маппинг между Category и CreateCategoryDTO
            CreateMap<Category, CreateCategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();

            // Маппинг промежуточной сущности CategoryProduct в CategoryResponseDTO
            // с получением данных из связанной сущности Category
            CreateMap<CategoryProduct, CategoryResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
