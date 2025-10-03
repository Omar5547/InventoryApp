using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                OrdersCount = 120,
                ProductsCount = 80,
                CustomersCount =44,
                SuppliersCount = 25,
                MonthlySales = new List<int> { 1000,1200,1100,1500,1700,1600 },

            };
            return View(model);
        }
    }
}
