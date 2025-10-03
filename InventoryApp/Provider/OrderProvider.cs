using Application.DTO;
using Application.Interfaces;
using Core.Entities;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace InventoryApp.Provider
{
    public class PurchaseOrderProvider
    {
        private readonly IPurchaseOrderService _orderService;
        private readonly IVendorService _vendorService;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _http;

        public PurchaseOrderProvider(IPurchaseOrderService orderService,
            IVendorService vendorService,
            IProductService productService
            )
        {
            _orderService = orderService;
            _vendorService = vendorService;
            _productService = productService;
           
        }
       
        public async Task<List<PurchaseOrderViewModel>> GetAllAsync(string? search=null)
        {
            var dtos = await _orderService.GetAllAsync(search);
            return dtos.Select(order => new PurchaseOrderViewModel
            {
                Id = order.Id,
                VendorName = order.VendorName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Currency = order.Currency,
                Discount = order.Discount,
                NonVendorCosts = order.NonVendorCosts

            }).ToList();
            
        }

        public async Task<PurchaseOrderViewModel?> GetByIdAsync(int id) 
        {
            var dto = await _orderService.GetByIdAsync(id);
            if (dto == null) return null;
            var vendors = await _vendorService.GetAllAsync();
            var items = await GetOrderItemsAsync(id);
            return new PurchaseOrderViewModel
            {
                Id = dto.Id,
                VendorId = dto.VendorId,
                VendorName = dto.VendorName,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                Currency = dto.Currency,
                Discount = dto.Discount,
                NonVendorCosts = dto.NonVendorCosts,
                Items = items,
                VendorList = vendors.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }).ToList()
            };


        }
          
        public async Task<PurchaseOrderViewModel> GetEmptyWithVendorsAsync()
        {
            var vendors = await _vendorService.GetAllAsync();
            return new PurchaseOrderViewModel
            {
                VendorList = vendors.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }).ToList(),

            };
        }

        public async Task<int> CreateAsync(PurchaseOrderViewModel model)
        {
            var orderDto = new PurchaseOrderDto
            {
                VendorId = model.VendorId,
                OrderDate = model.OrderDate,
                Discount  = model.Discount,
                Currency = model.Currency,
                NonVendorCosts = model.NonVendorCosts,
                Status = model.Status,
                IsActive = model.IsActive


            };
            var items = model.Items.Select(i => new PurchOrderItemDto
            {
                ProductId = i.ProductId,
                Qty = i.Qty,
                UnitPrice = i.UnitPrice,
                 Discount = i.Discount
            }).ToList();  
            return await _orderService.AddAsync(orderDto, items);
        }
        public async Task UpdateAsync(PurchaseOrderViewModel model)
        {
            var dto = new PurchaseOrderDto
            {
                Id = model.Id,
                VendorId = model.VendorId,
                OrderDate = model.OrderDate,
                Discount = model.Discount,
                Currency = model.Currency,
                NonVendorCosts = model.NonVendorCosts,
                Status = model.Status,
                IsActive = model.IsActive
            };
            await _orderService.UpdateAsync(dto);
        }
        public async Task DeleteAsync(int id) 
        {
            await _orderService.DeleteAsync(id);
        }
        public async Task PostAsync (int id , int locationId)
        {
            await _orderService.PostAsync(id, locationId);
        }
        public async Task CancelAsync(int id)
        {
            await _orderService.CancelAsync(id);
        }
        public async Task AddItemAsync (PurchaseOrderItemViewModel item)
        {
            var dto = new PurchOrderItemDto
            {
                PurchOrderId = item.Id,
                ProductId = item.ProductId,
                Qty = item.Qty,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount
            };
            await _orderService.AddOrUpdateItemAsync(dto);
        }
        public async Task RemoveItemAsyncs(int itemId)
        {
            await _orderService.DeleteAsync(itemId);
        }
        public async Task UpdateItemAsync (PurchaseOrderItemViewModel item)
        {
            var dto = new PurchOrderItemDto
            {
                PurchOrderId = item.Id,
                ProductId = item.ProductId,
                Qty = item.Qty,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount
            };
            await _orderService.AddOrUpdateItemAsync(dto);
        }
        private async Task<List<PurchaseOrderItemViewModel>> GetOrderItemsAsync(int orderId)
        { 
            return new List<PurchaseOrderItemViewModel>();
        }
       
        }
    }

