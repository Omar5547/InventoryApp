using Core.Entities;
using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        private readonly ILogger<HomeController> _logger;
        private readonly ProductProvider _productProvider;
        private readonly CategoryProvider _categoryProvider;
        private readonly CustomerProvider _customerProvider;

        public HomeController(ILogger<HomeController> logger ,  ProductProvider productProvider , 
            CategoryProvider categoryProvider,
            CustomerProvider customerProvider
            )
        {
            _logger = logger;
            _productProvider = productProvider;
            _categoryProvider = categoryProvider;
            _customerProvider = customerProvider;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryProvider.GetAllAsync();
            var products = await _productProvider.GetAllAsync();
            var latestProducts = products.OrderByDescending(p => p.Id).Take(9);

            var customers = await _customerProvider.GetAllAsync();
            var reviews = customers.Select(c => new ReviewViewModel
            {
                // عدّل الأسماء حسب خصائص CustomerViewModel عندك
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Subject = "عميل لدينا",              // قيمة افتراضية
            });

            var vm = new HomeIndexViewModel
            {
                Categories = categories,
                Products = products,
                LatestProducts = latestProducts,
                Reviews = reviews
            };

            return View(vm);
        }
        public async Task<IActionResult> Details(int id)
        {
            var products = await _productProvider.GetAllAsync();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);

        }
        [HttpGet]
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
