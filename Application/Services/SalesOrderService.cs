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
        private readonly IGenericRepository<Product> _productRepo;

        public SalesOrderService(
            IGenericRepository<SalesOrder> salesorderRepo,
            IGenericRepository<Customer> customerRepo,
            IGenericRepository<SalesOrderItem> ItemRepo,
            IGenericRepository<Product> productRepo
            ) 
        {
            _salesorderRepo = salesorderRepo;
            _customerRepo = customerRepo;
            _itemRepo = ItemRepo;
            _productRepo = productRepo;
        }
        public async Task<int> AddAsync(SalesOrderDto salesOrderDto, IEnumerable<SalesOrderItemDto> items)
        {
            var customer = await _customerRepo.Query().AsNoTracking().AnyAsync(c => c.Id == salesOrderDto.CustomerId);
            var salesorder = new SalesOrder
            {
                CustomerId = salesOrderDto.CustomerId,
                OrderDate = salesOrderDto.OrderDate,
                Currency = salesOrderDto.Currency,
                Status = OrderStatus.Open,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SalesOrderItems = items.Select(i => new SalesOrderItem
                {
                    ProductId = i.ProductId,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    Tax = i.Tax,
                    Total = i.Qty * i.UnitPrice * (1 - i.Discount / 100) * (1 + i.Tax / 100),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }).ToList()

            };
            await _salesorderRepo.AddAsync(salesorder);
            await _salesorderRepo.SaveAsync();
            foreach (var item in salesorder.SalesOrderItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);
                }
            }
            await _productRepo.SaveAsync();
            return salesorder.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var salesorder = await _salesorderRepo.GetByIdAsync(id);
            if (salesorder == null) return;
            foreach (var item in salesorder.SalesOrderItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);
                }
            }
            _salesorderRepo.Delete(salesorder);
            await _salesorderRepo.SaveAsync();
            await _productRepo.SaveAsync();
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
                       Tax = i.Tax,
                      Total = i.Total
                   }).ToList()
                   };

        }

      
        

     

     
       

        public async Task UpdateAsync(SalesOrderDto salesOrderDto, IEnumerable<SalesOrderItemDto> items)
        {
            var salesorder = await _salesorderRepo.GetByIdAsync(salesOrderDto.Id);
            if (salesorder == null) return;

            salesorder.CustomerId = salesOrderDto.CustomerId;
            salesorder.OrderDate = salesOrderDto.OrderDate;
            salesorder.Currency = salesOrderDto.Currency;

            salesorder.Status = salesOrderDto.Status;
            salesorder.IsActive = salesOrderDto.IsActive;
           
            salesorder.UpdatedAt = DateTime.UtcNow;
            var exisitingItems = await _itemRepo.Query().Where(i => i.SalesOrderId == salesorder.Id).ToListAsync();
            foreach (var item in exisitingItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);
                }
                _itemRepo.Delete(item);

            }

            foreach (var item in items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);
                }
                var orderItem = new SalesOrderItem
                {
                    SalesOrderId = salesorder.Id,
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    Total = item.Qty * item.UnitPrice * (1 - item.Discount / 100M) * (1 + item.Tax / 100M),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };
                await _itemRepo.AddAsync(orderItem);
            }
            _salesorderRepo.Update(salesorder);
            await _salesorderRepo.SaveAsync();
        }

       public async Task<IEnumerable<SalesOrderListItemDto>> GetAllAsync(string? search)
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


                             };
            if (!string.IsNullOrEmpty(search))
            {
                salesorder = salesorder.Where(s => s.CustomerName.Contains(search));
            }
            return salesorder.ToList();
        }
    }
}
