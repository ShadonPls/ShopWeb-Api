using AutoMapper;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;
using ShopWeb_Api.Models.DTO.Category;
using ShopWeb_Api.Models.DTO.Order;
using ShopWeb_Api.Models.DTO.Product;
using ShopWeb_Api.Models.DTO.User;

namespace ShopWeb_Api.Services.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDTO>().ForMember(dest => dest.Roles,opt => opt.MapFrom(src => src.Roles.Select(ru => ru.Role.Name)));

            CreateMap<Product, ProductResponseDTO>();
            CreateMap<Product, UpdateProductDTO>();
            CreateMap<UpdateProductDTO, Product >();
            CreateMap<CategoryProduct, CategoryDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, CreateProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Category, CreateCategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<Cart, CartResponseDTO>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<Order, OrderResponseDTO>().ForMember(dest=>dest.Items, opt=>opt.MapFrom(src=>src.OrderItems));
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price));
            CreateMap<OrderResponseDTO,Order >();
        }
    }
}
