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

        public Task<bool> ExistsSkuAsync(string sku, int? excludeId = null)
        {
            sku = sku.Trim();
            var query = _productRepo.Query().AsNoTracking()
                .Where(p => p.SKU == sku);
            if (excludeId.HasValue) query = query.Where(p => p.Id != excludeId.Value);
            return query.AnyAsync();
        }

       

        public async Task<IReadOnlyList<ProductListItemDto>> GetAllAsync(string? search = null, int? skip = null, int? take = null)
        {
            var products =  _productRepo.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Name.Contains(search) || (p.SKU.Contains(search)));
            }
            
            var result = from p in products
                         join c in _categoryRepo.Query().AsNoTracking() on p.CategoryId equals c.Id into pc
                         from c in pc.DefaultIfEmpty()
                         orderby p.Name
                         select new ProductListItemDto
                         {
                             Id = p.Id,
                             Name = p.Name,
                                SKU = p.SKU,
                             CategoryName = c != null ? c.Name : string.Empty,
                             SalePrice = p.SalePrice,
                             Unit = p.Unit,
                             IsActive = p.IsActive,
                         };
            if (skip.HasValue) result = result.Skip(skip.Value);
            if (take.HasValue) result = result.Take(take.Value);
            return await result.ToListAsync();

        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _productRepo.Query().AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    SKU = p.SKU,
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
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
            product.SKU = productDto.SKU;
            product.Name = productDto.Name;
            product.PurchasePrice = productDto.PurchasePrice;
            product.SalePrice = productDto.SalePrice;
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
                SKU = productDto.SKU,
                Name = productDto.Name,
                PurchasePrice = productDto.PurchasePrice,
                SalePrice = productDto.SalePrice,
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

    }
}
