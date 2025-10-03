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
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<PurchaseOrder> _orderRepo;
        private readonly IGenericRepository<SalesOrder> _salesOrder;

        public ReportService(IGenericRepository<PurchaseOrder> orderRepo,
            IGenericRepository<SalesOrder> salesOrder
            )
        {
            _orderRepo = orderRepo;
            _salesOrder = salesOrder;
        }


      
        public async Task<List<SalesReportDto>> GetSalesReportsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var salesOrder =  _salesOrder.Query()
                .Include(o => o.Customer)
                .Include(o => o.SalesOrderItems).AsNoTracking();
            if (startDate.HasValue)
                salesOrder = salesOrder.Where(o => o.OrderDate >= startDate.Value);
            if (endDate.HasValue)
                salesOrder = salesOrder.Where(o => o.OrderDate <= endDate.Value);
            var orders = await salesOrder.ToListAsync();
            return orders.Select(o => new SalesReportDto
            {
                OrderId = o.Id,
                CustomerName = o.Customer?.Name ?? "Unknown",
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                Discount = o.Discount,

                TotalAmount = o.SalesOrderItems.Sum(i => i.Qty * i.UnitPrice) - o.Discount
            }).ToList();
        }

        public async Task<List<OrderReportDto>> GetOrderReportsAsync(DateTime? startDate, DateTime? endDate)
        {
            var orders = _orderRepo.Query()
                .Include(o => o.Vendor)
                .Include(o => o.Items).AsNoTracking();
            if (startDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            if (endDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= endDate.Value);
            var orderList = await orders.ToListAsync();
            return orderList.Select(o => new OrderReportDto
            {
                OrderId = o.Id,
                VendorName = o.Vendor?.Name ?? "Unknown",
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                Discount = o.Discount,
                TotalAmount = o.Items.Sum(i => i.Qty * i.UnitPrice) 
            }).ToList();
        }
    }
}
