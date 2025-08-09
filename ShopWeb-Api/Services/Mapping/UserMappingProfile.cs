using AutoMapper;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.User;

namespace ShopWeb_Api.Services.Mapping
{
    /// <summary>
    /// Профиль маппинга для преобразования между сущностью User и DTO
    /// </summary>
    public class UserMappingProfile : Profile
    {
        /// <summary>
        /// Конфигурирует маппинг между User и UserResponseDTO
        /// </summary>
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.Roles,
                          opt => opt.MapFrom(src => src.Roles.Select(ru => ru.Role.Name)));
        }
    }
}
