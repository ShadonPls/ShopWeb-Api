using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Cart;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Services
{
    public class CartService : ICartService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;

        public CartService(Data.AppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartResponseDTO> GetUserCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<CartResponseDTO>(cart);
        }

        public async Task AddItemToCartAsync(int userId, AddCartItemDTO itemDto)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId)
                ?? new Cart { UserId = userId };

            var product = await _context.Products.FindAsync(itemDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromCartAsync(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item == null)
                throw new KeyNotFoundException("Cart item not found");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
        }
    }
}
