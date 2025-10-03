using Application.Interfaces;
using InventoryApp.ViewModel;

namespace InventoryApp.Provider
{
    public class ReportProvider
    {
        private readonly IReportService _reportService;

        public ReportProvider(IReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<List<PurchaseReportViewModel>> GetReportOrdersAsync(DateTime? startDate = null , DateTime? endDate = null) 
        {
            var dtos =await _reportService.GetOrderReportsAsync(startDate,endDate);
            return dtos.Select(x => new PurchaseReportViewModel
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                VendorName = x.VendorName,
                TotalAmount = x.TotalAmount,
                Discount = x.Discount,
                Status = x.Status

            }).ToList();
        }
        public async Task<List<SalesReportViewModel>> GetReportSalesAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var dtos = await _reportService.GetSalesReportsAsync(startDate, endDate);
            return dtos.Select(x => new SalesReportViewModel
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                CustomerName = x.CustomerName,
                TotalAmount = x.TotalAmount,
                Discount = x.Discount,
                Status = x.Status
            }).ToList();
        }
    }
}
