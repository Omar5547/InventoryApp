using Application.DTO;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PurchOrderService : IPurchaseOrderService
    {
        private readonly IGenericRepository<PurchaseOrder> _orderRepo;
        private readonly IGenericRepository<Vendor> _vendorRepo;
        private readonly IGenericRepository<PurchOrderItem> _orderItemRepo;
        private readonly IGenericRepository<Product> _productRepo;


        public PurchOrderService(
            IGenericRepository<PurchaseOrder> orderRepo,
            IGenericRepository<Vendor> vendorRepo,
            IGenericRepository<PurchOrderItem> orderItemRepo,
            IGenericRepository<Product> productRepo)
        {
            _orderRepo = orderRepo;

            _vendorRepo = vendorRepo;
            _orderItemRepo = orderItemRepo;
            _productRepo = productRepo;

        }



        public async Task<int> AddAsync(PurchaseOrderDto orderDto, IEnumerable<PurchOrderItemDto> items)
        {
            var vendor = await _vendorRepo.Query().AsNoTracking().AnyAsync(v => v.Id == orderDto.VendorId);
            var order = new PurchaseOrder
            {
                VendorId = orderDto.VendorId,
                OrderDate = orderDto.OrderDate,
                Currency = orderDto.Currency,
                Status = orderDto.Status,
                IsActive = orderDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = items.Select(i => new PurchOrderItem
                {
                    ProductId = i.ProductId,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice,
                    Tax = i.Tax,
                    Discount = i.Discount,
                    Total = (i.Qty * i.UnitPrice) * (1 - i.Discount / 100) * (1 + i.Tax / 100),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true

                }).ToList()
            };
            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveAsync();
            foreach (var item in order.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);
                }
            }
            await _orderRepo.SaveAsync();
            return order.Id;

        }




        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return;
            foreach (var item in order.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);
                }
            }
            _orderRepo.Delete(order);

            await _orderRepo.SaveAsync();
            await _productRepo.SaveAsync();
        }



        public async Task<PurchaseOrderDto?> GetByIdAsync(int id)
        {
            var purchaseorder = _orderRepo.Query().AsNoTracking()
                .Include(p => p.Vendor)
                .Include(p => p.Items).
                ThenInclude(i => i.Product)
                .FirstOrDefault(p => p.Id == id);
            if (purchaseorder == null) return null;
            return new PurchaseOrderDto
            {
                Id = purchaseorder.Id,
                VendorId = purchaseorder.VendorId,
                VendorName = purchaseorder.Vendor != null ? purchaseorder.Vendor.Name : string.Empty,
                OrderDate = purchaseorder.OrderDate,
               
                Currency = purchaseorder.Currency,
                Status = purchaseorder.Status,
                IsActive = purchaseorder.IsActive,
                CreatedAt = purchaseorder.CreatedAt,
                UpdatedAt = purchaseorder.UpdatedAt,
                Items = purchaseorder.Items.Select(i => new PurchOrderItemDto
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

        public async Task UpdateAsync(PurchaseOrderDto orderDto, IEnumerable<PurchOrderItemDto> items)
        {
            var order = await _orderRepo.GetByIdAsync(orderDto.Id);
            if (order == null) return;

            order.VendorId = orderDto.VendorId;
            order.OrderDate = orderDto.OrderDate;

           
            order.Currency = orderDto.Currency;
            order.Status = orderDto.Status;
            order.IsActive = orderDto.IsActive;
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            var excistingItems = await _orderItemRepo.Query().Where(i => i.PurchOrderId == order.Id).ToListAsync();
            foreach (var item in excistingItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);
                }
                _orderItemRepo.Delete(item);
            }
            foreach (var item in items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Qty;
                    product.UpdatedAt = DateTime.UtcNow;
                    _productRepo.Update(product);


                }
                var orderItem = new PurchOrderItem
                {
                    PurchOrderId = order.Id,
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    Tax = item.Tax,
                    Discount = item.Discount,
                    Total = (item.Qty * item.UnitPrice) * (1 - item.Discount / 100) * (1 + item.Tax / 100),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                await _orderItemRepo.AddAsync(orderItem);
            }

            _orderRepo.Update(order);
            await _orderRepo.SaveAsync();
            await _productRepo.SaveAsync();
        }

       public async Task<IEnumerable<PurchaseOrderListItemDto>> GetAllAsync(string? search)
        {
            var order = from o in _orderRepo.Query().AsNoTracking()

                        join v in _vendorRepo.Query().AsNoTracking() on o.VendorId equals v.Id into ov
                        from v in ov.DefaultIfEmpty()
                        orderby o.OrderDate descending
                        select new PurchaseOrderListItemDto
                        {
                            Id = o.Id,
                            VendorName = v != null ? v.Name : string.Empty,
                            OrderDate = o.OrderDate,
                            Status = o.Status,
                            Currency = o.Currency,
                         Total = o.Items.Sum(i => i.Total)


                        };
            if (!string.IsNullOrEmpty(search))
            {
                order = order.Where(o => o.VendorName.Contains(search));
            }
            return order.ToList();
        }
    }
  }
