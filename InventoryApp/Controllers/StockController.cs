using InventoryApp.Provider;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class StockController : Controller
    {
        private readonly StockProvider _stockProvider;

        public StockController(StockProvider stockProvider) 
        {
            _stockProvider = stockProvider;
        }
        public async Task <IActionResult> Index(string search)
        {
            var Model = await _stockProvider.GetAllStockAsync(search);
            return View(Model);
        }
        public async Task<IActionResult> ByProduct(int productId , string search)
        {
            var Model = await _stockProvider.GetStockByProductAsync(productId, search);
            return View("Index", Model);
        }
        public async Task<IActionResult> ByLocation(int locationId, string search)
        {
            var Model = await _stockProvider.GetStockByLocationAsync(locationId, search);
            return View("Index", Model);
        }
    }
}
