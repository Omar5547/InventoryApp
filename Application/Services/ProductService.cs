using Application.DTO;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Category> _categoryRepo;

        public ProductService(IGenericRepository<Product> productRepo,
            IGenericRepository<Category> categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }
        
        public async Task DeleteAsync(int id)
        {
           var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return;
            _productRepo.Delete(product);
            await _productRepo.SaveAsync();
        }

      

       


        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _productRepo.Query().AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                    Quantity =p.Quantity,
                    SalePrice = p.SalePrice,
                    Unit = p.Unit,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                }).FirstOrDefaultAsync();
            
        }

      

        
        

        public async Task UpdateAsync(ProductDto productDto)
        {
            var product = await _productRepo.GetByIdAsync(productDto.Id);
            if (product == null) return;
            product.Code = productDto.Code;
            product.Name = productDto.Name;
            product.SalePrice = productDto.SalePrice;
            product.Quantity = productDto.Quantity;
            product.Description = productDto.Description;

            product.Unit = productDto.Unit;
            product.IsActive = productDto.IsActive;
            product.UpdatedAt = productDto.UpdatedAt;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId;
            _productRepo.Update(product);
            await _productRepo.SaveAsync();
        }

       

        public async Task<int> AddAsync(ProductDto productDto)
        {
            var Product = new Product
            {
                Code = productDto.Code,
                Name = productDto.Name,
                
                SalePrice = productDto.SalePrice,
                Quantity = productDto.Quantity,
                Description = productDto.Description,

                Unit = productDto.Unit,
                CategoryId = productDto.CategoryId,
                IsActive = productDto.IsActive,
                ImageUrl = productDto.ImageUrl,
               
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            await _productRepo.AddAsync(Product);
            await _productRepo.SaveAsync();
            return Product.Id;
        }

       

        public async Task<IEnumerable<ProductDto>> GetAllAsync(string? search)
        {
            var query = _productRepo.Query().AsNoTracking().Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Code.Contains(search));
            }

            var list = query
                .OrderBy(p => p.Name)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    SalePrice = p.SalePrice,
                    Quantity = p.Quantity,
                    Unit = p.Unit,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name ?? string.Empty,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToList();

            return list;
        }
    }
}
