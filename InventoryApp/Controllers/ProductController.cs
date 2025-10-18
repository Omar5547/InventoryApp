using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly ProductProvider _productProvider;

        public ProductController(ProductProvider productProvider)
        {
            _productProvider = productProvider;
        }
        // GET: ProductController
        public async Task<IActionResult> Index(string search)
        {
            var products = await _productProvider.GetAllAsync(search); 
            return View(products);
        }
    
        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productProvider.GetByIdAsync(id);
           if (product == null) return NotFound();
            return View(product);
        }

        // GET: ProductController/Create
        public async Task <IActionResult> Create()
        {
            var model = await _productProvider.GetCreateAsync();
            return View(model);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if(!ModelState.IsValid)
                return View (model);
            await _productProvider.CreateAsync(model);
            return RedirectToAction(nameof(Index));
            
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _productProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(int id, ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            await _productProvider.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _productProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            await _productProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
