using AutoMapper;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;

namespace ShopWeb_Api.Services.Mapping
{
    /// <summary>
    /// Профиль маппинга для преобразования между сущностями Cart, CartItem и соответствующими DTO
    /// </summary>
    public class CartMappingProfile : Profile
    {
        /// <summary>
        /// Конфигурирует маппинги для корзины и её элементов
        /// </summary>
        public CartMappingProfile()
        {
            // Маппинг Cart в CartResponseDTO с преобразованием элементов корзины
            CreateMap<Cart, CartResponseDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));

            // Маппинг CartItem в CartItemResponseDTO с доп. информацией о продукте
            CreateMap<CartItem, CartItemResponseDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));
        }
    }
}
