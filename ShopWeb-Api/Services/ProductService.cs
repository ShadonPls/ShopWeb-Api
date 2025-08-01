using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.Product;
using ShopWeb_Api.Services.Interfaces;
using System;

namespace ShopWeb_Api.Services
{
    public class ProductService : IProductService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;

        public ProductService(Data.AppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductResponseDTO> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .ThenInclude(cp => cp.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) throw new KeyNotFoundException("Product not found");

            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Categories)
                .ThenInclude(cp => cp.Category)
                .ToListAsync();

            return _mapper.Map<List<ProductResponseDTO>>(products);
        }

        public async Task<ProductResponseDTO> CreateProductAsync(CreateProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductResponseDTO>(product);
        }
        public async Task UpdateProductAsync(int id, UpdateProductDTO productDto)
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            _mapper.Map(productDto, product);

            // Обновляем категории (если переданы)
            if (productDto.CategoryIds != null)
            {
                product.Categories.Clear();
                foreach (var categoryId in productDto.CategoryIds)
                {
                    product.Categories.Add(new CategoryProduct { CategoryId = categoryId });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
