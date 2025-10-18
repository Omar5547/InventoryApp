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
                Total = order.Total,
                


            }).ToList();
            
        }

        public async Task<PurchaseOrderViewModel?> GetByIdAsync(int id) 
        {
            var dto = await _orderService.GetByIdAsync(id);
            if (dto == null) return null;
            var vendors = await _vendorService.GetAllAsync();
            var items = await GetOrderItemsAsync(id);
            var products = await _productService.GetAllAsync();
            return new PurchaseOrderViewModel
            {
                Id = dto.Id,
                VendorId = dto.VendorId,
                VendorName = dto.VendorName,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                Currency = dto.Currency,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,


                Items = dto.Items.Select(i => new PurchaseOrderItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    Tax = i.Tax,
                    Total = i.Total,
                }).ToList() ?? new List<PurchaseOrderItemViewModel>(),
                VendorList = vendors.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = v.Id == dto.VendorId
                }).ToList(),
                ProductList = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    SalePrice = p.SalePrice
                }).ToList(),
            };


        }
          
        public async Task<PurchaseOrderViewModel> GetEmptyWithVendorsAsync()
        {
            var vendors = await _vendorService.GetAllAsync();
            var products = await _productService.GetAllAsync();
            return new PurchaseOrderViewModel
            {
                ProductList = products.Select(p => new ProductViewModel
               {
                   Id = p.Id,
                   Name = p.Name,
                   SalePrice = p.SalePrice
               }).ToList(),
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
                Currency = model.Currency,
                Status = model.Status,
                IsActive = model.IsActive


            };
            var items = model.Items.Select(i => new PurchOrderItemDto
            {
                ProductId = i.ProductId,
                Qty = i.Qty,
                UnitPrice = i.UnitPrice,
                 Discount = i.Discount,
                 Tax = i.Tax,
                Total = i.Qty * i.UnitPrice * (1 - i.Discount / 100) * (1 + i.Tax / 100)
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
                Currency = model.Currency,
                Status = model.Status,
                IsActive = model.IsActive
            };
            var items = model.Items.Select(i => new PurchOrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Qty = i.Qty,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                Tax = i.Tax,
                Total = i.Qty * i.UnitPrice * (1 - i.Discount / 100) * (1 + i.Tax / 100)
            }).ToList();
            await _orderService.UpdateAsync(dto , items);
        }
        public async Task DeleteAsync(int id) 
        {
            await _orderService.DeleteAsync(id);
        }
        
        
        private async Task<List<PurchaseOrderItemViewModel>> GetOrderItemsAsync(int orderId)
        { 
            return new List<PurchaseOrderItemViewModel>();
        }
       
        }
    }

