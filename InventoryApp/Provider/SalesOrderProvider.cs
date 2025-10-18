using Application.DTO;
using Application.Interfaces;
using Application.Services;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.Provider
{
    public class SalesOrderProvider
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public SalesOrderProvider
            (
                ISalesOrderService salesOrderService,
                ICustomerService customerService,
                IProductService productService
            )
        {
            _salesOrderService = salesOrderService;
            _customerService = customerService;
            _productService = productService;
        }
        public async Task<List<SalesOrderViewModel>> GetAllAsync(string? search = null)
        {
            var dtos = await _salesOrderService.GetAllAsync(search);
            return dtos.Select(dto => new SalesOrderViewModel
            {
                Id = dto.Id,
                CustomerName = dto.CustomerName,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                Currency = dto.Currency,
                


            }).ToList();

        }
        public async Task<SalesOrderViewModel?> GetByIdAsync(int id)
        {
            var dto = await _salesOrderService.GetByIdAsync(id);
            if (dto == null) return null;
            var customer = await _customerService.GetAllAsync();
            var products = await _productService.GetAllAsync();

            return new SalesOrderViewModel
            {
                Id = dto.Id,
                CustomerId = dto.CustomerId,
                CustomerName = customer.FirstOrDefault(c => c.Id == dto.CustomerId)?.Name ?? "Unknown",
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                Currency = dto.Currency,
                
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                OrderItems = dto.Items?.Select(i => new SalesOrderItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    Tax = i.Tax,
                   
                    
                    
                }).ToList()?? new List<SalesOrderItemViewModel>(),
                CustomerList = customer.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = c.Id == dto.CustomerId

                }).ToList(),
                ProductList = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    SalePrice = p.SalePrice,

                }).ToList()


            };

        }
        public async Task<SalesOrderViewModel> GetEmptyWithCustomerAsync()
        {
            var customers = await _customerService.GetAllAsync();
            var products = await _productService.GetAllAsync();
            return new SalesOrderViewModel
            {
                ProductList = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    SalePrice = p.SalePrice,
                   
                }).ToList(),
                CustomerList = customers.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

        }
    
        public async Task<int> CreateAsync(SalesOrderViewModel model)
        {
            var orderDto = new SalesOrderDto
            {
                CustomerId = model.CustomerId,
                OrderDate = model.OrderDate,
                Status = model.Status,
                Currency = model.Currency,
                
                IsActive = model.IsActive
            };
            var items = model.OrderItems.Select(i => new SalesOrderItemDto
            {
                ProductId = i.ProductId,
                Qty = i.Qty,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                Tax = i.Tax,
                Total = (i.Qty *i.UnitPrice) * (1-i.Discount/100) * (1 + i.Tax / 100),
            }).ToList();
            return await _salesOrderService.AddAsync(orderDto, items);
        }
        public async Task UpdateAsync(SalesOrderViewModel model)
        {

            var orderDto = new SalesOrderDto
            {
                Id = model.Id,
                CustomerId = model.CustomerId,
                OrderDate = model.OrderDate,
                Status = model.Status,
                Currency = model.Currency,
                
                IsActive = model.IsActive,
                
            };
            var items = model.OrderItems.Select(i => new SalesOrderItemDto
            {
               
                ProductId = i.ProductId,
                Qty = i.Qty,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                Tax = i.Tax,
                Total = i.Qty * i.UnitPrice * (1 - i.Discount / 100M) * (1 + i.Tax / 100M),
            }).ToList();
            
            await _salesOrderService.UpdateAsync(orderDto,items);

        }
        public async Task DeleteAsync(int id)
        {
            await _salesOrderService.DeleteAsync(id);
        }
        
       
    }
}

