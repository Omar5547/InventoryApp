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
    public class OrderService : IPurchaseOrderService
    {
        private readonly IGenericRepository<PurchaseOrder> _orderRepo;
        private readonly IGenericRepository<Vendor> _vendorRepo;
        private readonly IGenericRepository<PurchOrderItem> _orderItemRepo;
        private readonly IStockService _stockService;

        public OrderService(
            IGenericRepository<PurchaseOrder> orderRepo,
            IGenericRepository<Vendor> vendorRepo,
            IGenericRepository<PurchOrderItem> orderItemRepo,
            IStockService stockService)
        {
            _orderRepo = orderRepo;
            
            _vendorRepo = vendorRepo;
            _orderItemRepo = orderItemRepo;
            _stockService = stockService;
        }

      

        public async Task<int> AddAsync(PurchaseOrderDto orderDto, IEnumerable<PurchOrderItemDto> items)
        {
            var vendor = await _vendorRepo.Query().AsNoTracking().AnyAsync(v => v.Id == orderDto.VendorId);
            var order = new PurchaseOrder
            {
                VendorId = orderDto.VendorId,
                OrderDate = orderDto.OrderDate,
                Discount = orderDto.Discount,
                NonVendorCosts = orderDto.NonVendorCosts,
                Currency = orderDto.Currency,
                Status = orderDto.Status,
                IsActive = orderDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
             await _orderRepo.AddAsync(order);
           
            
                foreach (var itemDto in items)
                {
                    var item = new PurchOrderItem
                    {
                        PurchOrderId = order.Id,
                        ProductId = itemDto.ProductId,
                        Qty = itemDto.Qty,
                        UnitPrice = itemDto.UnitPrice,
                       Discount = itemDto.Discount
                    };
                    await _orderItemRepo.AddAsync(item);
                }
                await _orderRepo.SaveAsync();
                return order.Id;

        }

        public async Task AddOrUpdateItemAsync(PurchOrderItemDto item)
        {
            var order = await _orderRepo.GetByIdAsync(item.PurchOrderId);
            if (order == null) return;
            var orderItem = await _orderItemRepo.Query()
                .Where(i => i.PurchOrderId == item.PurchOrderId && i.ProductId == item.ProductId)
                .FirstOrDefaultAsync();
            if (orderItem == null)
            {      orderItem = new PurchOrderItem
                {
                    PurchOrderId = item.PurchOrderId,
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount
                };
                await _orderItemRepo.AddAsync(orderItem);
            }
            else
            {
                orderItem.Qty = item.Qty;
                orderItem.UnitPrice = item.UnitPrice;
                orderItem.Discount = item.Discount;
                _orderItemRepo.Update(orderItem);
            }
        }

        public async Task CancelAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order.Status == OrderStatus.Canceled) return; 
            order.Status = OrderStatus.Canceled;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepo.Update(order);
            await _orderRepo.SaveAsync();


        }

      

        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return;

            // Correctly pass the `Order` entity to the `Delete` method
            _orderRepo.Delete(order);

            await _orderRepo.SaveAsync();
        }


        public async Task<IReadOnlyList<PurchaseOrderListItemDto>> GetAllAsync(string? search = null)
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
                            Discount = o.Discount,
                            NonVendorCosts = o.NonVendorCosts
                        };
            if (!string.IsNullOrEmpty(search))
                {
                order = order.Where(o => o.VendorName.Contains(search));
            }
            return order.ToList();
        }


       

        public async Task PostAsync(int orderId, int locationId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order.Status != OrderStatus.Open) return;
            var items = await _orderItemRepo.Query()
                .Where(i => i.PurchOrderId == orderId)
                .ToListAsync();
            foreach (var item in items)
            {
                await _stockService.AdjustStockAsync(item.ProductId, locationId, item.Qty, "PurchaseOrder", orderId);
            }
            order.Status = OrderStatus.Posted;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepo.Update(order);
            await _orderRepo.SaveAsync();
           
        }

        public async Task RemoveItemAsync(int purchOrderId, int productId)
        {
            var order = await _orderRepo.GetByIdAsync(purchOrderId);
            if (order == null) return;
            var item = await _orderItemRepo.Query()
                .Where(i => i.PurchOrderId == purchOrderId && i.ProductId == productId)
                .FirstOrDefaultAsync();
            _orderItemRepo.Delete(item);
            await _orderItemRepo.SaveAsync();
        }

        

        public async Task UpdateAsync(PurchaseOrderDto orderDto)
        {
            var order = await _orderRepo.GetByIdAsync(orderDto.Id);
            if (order == null) return;

            order.VendorId = orderDto.VendorId;
            order.OrderDate = orderDto.OrderDate;
            order.Discount = orderDto.Discount;
            order.NonVendorCosts = orderDto.NonVendorCosts;
            order.Currency = orderDto.Currency;
            order.Status = orderDto.Status;
            order.IsActive = orderDto.IsActive;
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
                

            _orderRepo.Update(order);
            await _orderRepo.SaveAsync();
        }

         public async Task<PurchaseOrderDto?>GetByIdAsync(int id)
        {
            return await _orderRepo.Query().AsNoTracking()
                .Where(o => o.Id == id)
                .Select(o => new PurchaseOrderDto
                {
                    Id = o.Id,
                    VendorId = o.VendorId,
                    VendorName = o.Vendor != null ? o.Vendor.Name : string.Empty,
                    OrderDate = o.OrderDate,
                    Discount = o.Discount,
                    NonVendorCosts = o.NonVendorCosts,
                    Currency = o.Currency,
                    Status = o.Status,
                    IsActive = o.IsActive,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                }).FirstOrDefaultAsync();
        }
    }
}
