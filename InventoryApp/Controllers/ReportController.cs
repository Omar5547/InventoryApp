using InventoryApp.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportProvider _reportProvider;

        public ReportController(ReportProvider reportProvider)
        {
            _reportProvider = reportProvider;
        }
        public async Task<IActionResult> Sales(DateTime? StartDate,DateTime?endDate)
        {
            var report = await _reportProvider.GetReportSalesAsync(StartDate,endDate);

            return View(report);
        }
        public async Task<IActionResult> Purchase(DateTime? StartDate, DateTime? endDate)
        {
            var report = await _reportProvider.GetReportOrdersAsync(StartDate, endDate);
            return View(report);
        }
    }
}
