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
    public class StockService : IStockService
    {
        private readonly IGenericRepository<ProductStock> _stockRepo;
        private readonly IGenericRepository<InventoryMoment> _momentRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Location> _locationRepo;

        public StockService(IGenericRepository<ProductStock> stockRepo ,
            IGenericRepository<InventoryMoment> momentRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<Location> locationRepo)
        {
            _stockRepo = stockRepo;
            _momentRepo = momentRepo;
            _productRepo = productRepo;
            _locationRepo = locationRepo;
        }
        public async Task AdjustStockAsync(int productId, int locationId, decimal deltaQty, string sourceType, int? sourceId)
        {
            var stock = await _stockRepo.Query()
                .Where(s => s.ProductId == productId && s.LocationId == locationId)
                .FirstOrDefaultAsync(); 

            if (stock == null)
            {
                stock = new ProductStock
                {
                    ProductId = productId,
                    LocationId = locationId,
                    Qty = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                await _stockRepo.AddAsync(stock); 
            }

            stock.Qty += deltaQty;
            stock.UpdatedAt = DateTime.UtcNow;
            _stockRepo.Update(stock);
            var moment = new InventoryMoment
            {
                ProductId = productId,
                LocationId = locationId,
                Qty = deltaQty,
                SourceType = sourceType,
                SourceId = sourceId,
                CreatedAt = DateTime.UtcNow,
                MovementDate = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _momentRepo.AddAsync(moment);
        }

        public async Task<List<ProductStockDto>> GetAllStockAsync()
        {
            var stocks = await _stockRepo.Query()
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Select(s => new ProductStockDto
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    ProductName = s.Product.Name,
                    LocationId = s.LocationId,
                    LocationName = s.Location.Name,
                    Qty = s.Qty
                })
                .ToListAsync();
            return stocks;


        }

        public async Task<decimal> GetAvailableStockAsync(int productId, int locationId)
        {
            return await _stockRepo.Query()
                .Where(s => s.ProductId == productId && s.LocationId == locationId)
                .Select(s => s.Qty)
                .FirstOrDefaultAsync();

        }
    }
}
