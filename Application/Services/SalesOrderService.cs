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
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IGenericRepository<SalesOrder> _salesorderRepo;
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IGenericRepository<SalesOrderItem> _itemRepo;
        private readonly IStockService _stock;

        public SalesOrderService(
            IGenericRepository<SalesOrder> salesorderRepo,
            IGenericRepository<Customer> customerRepo,
            IGenericRepository<SalesOrderItem> ItemRepo,
            IStockService stock
            ) 
        {
            _salesorderRepo = salesorderRepo;
            _customerRepo = customerRepo;
            _itemRepo = ItemRepo;
            _stock = stock;
        }
        public async Task<int> AddAsync(SalesOrderDto salesOrderDto, IEnumerable<SalesOrderItemDto> items)
        {
            var customer = await _customerRepo.Query().AsNoTracking().AnyAsync(c => c.Id == salesOrderDto.CustomerId);
            var salesorder = new SalesOrder
            {
                CustomerId = salesOrderDto.CustomerId,
                OrderDate = salesOrderDto.OrderDate,
                Discount = salesOrderDto.Discount,
                Currency = salesOrderDto.Currency,
                Status = salesOrderDto.Status,
                IsActive = salesOrderDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _salesorderRepo.AddAsync(salesorder);
            await _salesorderRepo.SaveAsync();


            foreach (var itemDto in items)
            {
                var item = new SalesOrderItem
                {
                    SalesOrderId = salesorder.Id,
                    ProductId = itemDto.ProductId,
                    Qty = itemDto.Qty,
                    UnitPrice = itemDto.UnitPrice,
                    Discount = itemDto.Discount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                await _itemRepo.AddAsync(item);
            }
            await _itemRepo.SaveAsync();
            return salesorder.Id;
        }

        public async Task AddItemAsync(SalesOrderItemDto item)
        {
            var salesorder = await _salesorderRepo.GetByIdAsync(item.SalesOrderId);
            if (salesorder == null) return;
            var orderItem = new SalesOrderItem
            {
                SalesOrderId = item.SalesOrderId,
                ProductId = item.ProductId,
                Qty = item.Qty,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _itemRepo.AddAsync(orderItem);  
        }

        public async Task CancelAsync(int orderId)
        {
            var salesorder = await _salesorderRepo.GetByIdAsync(orderId);
            if (salesorder.Status == OrderStatus.Posted) return;
            salesorder.Status = OrderStatus.Canceled;
            salesorder.UpdatedAt = DateTime.UtcNow;
            _salesorderRepo.Update(salesorder);
            await _salesorderRepo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var salesorder = await _salesorderRepo.GetByIdAsync(id);
            if (salesorder == null) return;
               
           _salesorderRepo.Delete(salesorder);
              await _salesorderRepo.SaveAsync();
        }

        public async Task<SalesOrderDto?> GetByIdAsync(int id)
        {
            var salesorder =  _salesorderRepo.Query().AsNoTracking()
                .Include(s => s.Customer)
                .Include(s => s.SalesOrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(s=>s.Id == id);
            if (salesorder == null) return null;
           
               return new SalesOrderDto
               {
                   Id = salesorder.Id,
                   CustomerId = salesorder.CustomerId,
                   CustomerName = salesorder.Customer?.Name,
                   OrderDate = salesorder.OrderDate,
                   Discount =salesorder.Discount,
                   Currency = salesorder.Currency,
                   Status = salesorder.Status,
                   IsActive = salesorder.IsActive,
                   CreatedAt = salesorder.CreatedAt,
                   UpdatedAt = salesorder.UpdatedAt,
                   Items = salesorder.SalesOrderItems.Select(i => new SalesOrderItemDto
                   {
                       Id = i.Id,
                       ProductId = i.ProductId,
                       ProductName = i.Product?.Name,
                       Qty = i.Qty,
                       UnitPrice = i.UnitPrice,
                       Discount = i.Discount,
                   }).ToList()
                   };

        }

        public async Task<IReadOnlyList<SalesOrderListItemDto>> ListAsync(string? search = null)
        {
            var salesorder = from s in _salesorderRepo.Query().AsNoTracking()

                        join c in _customerRepo.Query().AsNoTracking() on s.CustomerId equals c.Id into sc
                        from c in sc.DefaultIfEmpty()
                        orderby s.OrderDate descending
                        select new SalesOrderListItemDto
                        {
                            Id = s.Id,
                            CustomerName = c != null ? c.Name : string.Empty,
                            OrderDate = s.OrderDate,
                            Status = s.Status,
                            Currency = s.Currency,
                            Discount = s.Discount,
                            
                        };
            if (!string.IsNullOrEmpty(search))
            {
                salesorder = salesorder.Where(s => s.CustomerName.Contains(search));
            }
            return salesorder.ToList();

        }

        public async Task PostAsync(int orderId, int locationId)
        {
            var salesorder = await _salesorderRepo.GetByIdAsync(orderId);
            if (salesorder.Status != OrderStatus.Open) return;
            var items = await _itemRepo.Query()
                .Where(i => i.SalesOrderId == orderId)
                .ToListAsync();
            foreach (var item in items)
            {
                await _stock.AdjustStockAsync(item.ProductId, locationId, -item.Qty, sourceType: "SO_POST", sourceId: orderId);
            }
            salesorder.Status = OrderStatus.Posted;
            salesorder.UpdatedAt = DateTime.UtcNow;
            _salesorderRepo.Update(salesorder);
            await _salesorderRepo.SaveAsync();
        }

        public async Task RemoveItemAsync(int itemId)
        {
            var item = await _itemRepo.GetByIdAsync(itemId);
            var salesorder = await _salesorderRepo.GetByIdAsync(item.SalesOrderId);
            if (salesorder.Status != OrderStatus.Open) return;
            
            _itemRepo.Delete(item);
            await _itemRepo.SaveAsync();
        }

        public async Task UpdateAsync(SalesOrderDto salesOrderDto)
        {
            var salesorder = await _salesorderRepo.GetByIdAsync(salesOrderDto.Id);
            if (salesorder == null) return;

            salesorder.CustomerId = salesOrderDto.CustomerId;
            salesorder.OrderDate = salesOrderDto.OrderDate;
            salesorder.Discount = salesOrderDto.Discount;
            salesorder.Currency = salesOrderDto.Currency;

            salesorder.Status = salesOrderDto.Status;
            salesorder.IsActive = salesOrderDto.IsActive;
            salesorder.CreatedAt = DateTime.UtcNow;
            salesorder.UpdatedAt = DateTime.UtcNow;


            _salesorderRepo.Update(salesorder);
            await _salesorderRepo.SaveAsync();
        }

        public async Task UpdateItemAsync(SalesOrderItemDto item)
        {
           var salesitem = await _itemRepo.GetByIdAsync(item.Id);
           var salesorder = await _salesorderRepo.GetByIdAsync(salesitem.SalesOrderId);
            if (salesorder == null) return;
            item.ProductId = item.ProductId;
            item.Qty = item.Qty;
            item.UnitPrice = item.UnitPrice;
            item.Discount = item.Discount;
            item.UpdatedAt = DateTime.UtcNow;
            _itemRepo.Update(salesitem);
            await _itemRepo.SaveAsync();
        }

        
    }
}
