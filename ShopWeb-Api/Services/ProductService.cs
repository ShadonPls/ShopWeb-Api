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
    /// <summary>
    /// Сервис для работы с товарами
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly Data.AppContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        /// <summary>
        /// Конструктор сервиса товаров
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Автомаппер для преобразования DTO</param>
        /// <param name="logger">Логгер для записи событий</param>
        public ProductService(Data.AppContext context, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получает товар по указанному идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>DTO товара или null, если товар не найден</returns>
        public async Task<ProductResponseDTO> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .ThenInclude(cp => cp.Category)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (product == null)
            {
                var errorMessage = $"Категория по данному ID {id} не найден";
                _logger.LogWarning(errorMessage);
                return null;
            }

            _logger.LogInformation($"Категория по ID {id} успешно получена");
            return _mapper.Map<ProductResponseDTO>(product);
        }

        /// <summary>
        /// Получает список всех товаров
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список DTO товаров</returns>
        public async Task<List<ProductResponseDTO>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            var products = await _context.Products
                .Include(p => p.Categories)
                .ThenInclude(cp => cp.Category)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ProductResponseDTO>>(products);
        }

        /// <summary>
        /// Создает новый товар
        /// </summary>
        /// <param name="productDto">DTO с данными для создания товара</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>DTO созданного товара</returns>
        public async Task<ProductResponseDTO> CreateProductAsync(CreateProductDTO productDto, CancellationToken cancellationToken = default)
        {
            var product = _mapper.Map<Product>(productDto);
            _mapper.Map(productDto, product);

            if (productDto.CategoryIds != null)
            {
                foreach (var categoryId in productDto.CategoryIds)
                {
                    product.Categories.Add(new CategoryProduct { CategoryId = categoryId });
                }
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);


            _logger.LogInformation($"Категория успешно создана");
            return _mapper.Map<ProductResponseDTO>(product);
        }

        /// <summary>
        /// Обновляет данные товара
        /// </summary>
        /// <param name="id">Идентификатор обновляемого товара</param>
        /// <param name="productDto">DTO с обновленными данными товара</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции</returns>
        public async Task<OperationResult> UpdateProductAsync(int id, UpdateProductDTO productDto, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (product == null)
            {
                var errorMessage = $"Продукт по данному ID {id} не найден";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            _mapper.Map(productDto, product);

            if (productDto.CategoryIds != null)
            {
                product.Categories.Clear();
                foreach (var categoryId in productDto.CategoryIds)
                {
                    product.Categories.Add(new CategoryProduct { CategoryId = categoryId });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Категория успешно изменена");
            return OperationResult.Success();
        }

        /// <summary>
        /// Удаляет товар по указанному идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемого товар</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции</returns>
        public async Task<OperationResult> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
            if (product == null)
            {
                var errorMessage = $"Продукт по данному ID {id} не найден";
                _logger.LogWarning(errorMessage);
                return OperationResult.Failure(errorMessage);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Категория успешно удалена");
            return OperationResult.Success();
        }
    }
}
