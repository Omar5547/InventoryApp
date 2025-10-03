using Application.DTO;
using Application.Interfaces;
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
            var dtos = await _salesOrderService.ListAsync(search);
            return dtos.Select(dto => new SalesOrderViewModel
            {
                Id = dto.Id,
                CustomerName = dto.CustomerName,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                Currency = dto.Currency,
                Discount = dto.Discount,


            }).ToList();

        }
        public async Task<SalesOrderViewModel?> GetByIdAsync(int id)
        {
            var dto = await _salesOrderService.GetByIdAsync(id);
            if (dto == null) return null;
            var customer = await _customerService.GetAllAsync();
         
            return new SalesOrderViewModel
            {
                Id = dto.Id,
                CustomerId = dto.CustomerId,
                CustomerName = customer.FirstOrDefault(c => c.Id == dto.CustomerId)?.Name ?? "Unknown",
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                Currency = dto.Currency,
                Discount = dto.Discount,
                Items = dto.Items?.Select(i => new SalesOrderItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount
                }).ToList()?? new List<SalesOrderItemViewModel>(),
                CustomerList = customer.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = c.Id == dto.CustomerId

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
        private async Task<List<SalesOrderItemViewModel>> GetOrderItemsAsync(int orderId)
        {
            var dto = await _salesOrderService.GetByIdAsync(orderId);
            if (dto?.Items == null)
                return new List<SalesOrderItemViewModel>();
            return dto.Items.Select(i => new SalesOrderItemViewModel
            {
                Id = i.Id,
                
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Qty = i.Qty,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
            }).ToList();
        }
        public async Task<int> CreateAsync(SalesOrderViewModel model)
        {
            var orderDto = new SalesOrderDto
            {
                CustomerId = model.CustomerId,
                OrderDate = model.OrderDate,
                Status = model.Status,
                Currency = model.Currency,
                Discount = model.Discount,
                IsActive = model.IsActive
            };
            var items = model.Items.Select(i => new SalesOrderItemDto
            {
                ProductId = i.ProductId,
                Qty = i.Qty,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount
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
                Discount = model.Discount,
                IsActive = model.IsActive
            };
            await _salesOrderService.UpdateAsync(orderDto);

        }
        public async Task DeleteAsync(int id)
        {
            await _salesOrderService.DeleteAsync(id);
        }
        public async Task PostAsync(int id, int locationId)
        {
            await _salesOrderService.PostAsync(id, locationId);
        }
        public async Task CancelAsync(int id)
        {
            await _salesOrderService.CancelAsync(id);


        }
        public async Task AddItemAsync(SalesOrderItemViewModel item)
        {
            var itemDto = new SalesOrderItemDto
            {
                SalesOrderId = item.Id,
                ProductId = item.ProductId,
                Qty = item.Qty,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount
            };
            await _salesOrderService.AddItemAsync(itemDto);
        }
        public async Task RemoveItemAsync(int itemId)
        {
            await _salesOrderService.RemoveItemAsync(itemId);
        }
        public async Task UpdateItemAsync(SalesOrderItemViewModel item)
        {
            var itemDto = new SalesOrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Qty = item.Qty,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount
            };
            await _salesOrderService.UpdateItemAsync(itemDto);
        }
    }
}

