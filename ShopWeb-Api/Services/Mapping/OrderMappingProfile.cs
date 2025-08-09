using AutoMapper;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Order;

namespace ShopWeb_Api.Services.Mapping
{
    /// <summary>
    /// Профиль маппинга для преобразования между сущностями Order, OrderItem и соответствующими DTO
    /// </summary>
    public class OrderMappingProfile : Profile
    {
        /// <summary>
        /// Конфигурирует маппинги для заказов и позиций заказа
        /// </summary>
        public OrderMappingProfile()
        {
            // Маппинг Order в OrderResponseDTO с преобразованием Items
            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

            // Маппинг OrderItem в OrderItemResponseDTO с доп. информацией о продукте
            CreateMap<OrderItem, OrderItemResponseDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price));

            // Обратный маппинг из OrderResponseDTO в Order
            CreateMap<OrderResponseDTO, Order>();
        }
    }
}
