using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryProvider _categoryProvider;

        public CategoryController(CategoryProvider categoryProvider)
        {
            _categoryProvider = categoryProvider;
        }
        // GET: CategoryController
        public async Task <IActionResult> Index(string search)
        {
            var categories = await _categoryProvider.GetAllAsync(search);
            return View(categories);
           
        }

        // GET: CategoryController/Details/5
        public async Task <IActionResult> Details(int id)
        {
            var categories = await _categoryProvider.GetByIdAsync(id);
            if (categories == null) return NotFound();
            return View(categories);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _categoryProvider.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _categoryProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _categoryProvider.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task <IActionResult> Delete(int id)
        {
            var model = await _categoryProvider.GetByIdAsync(id);
            if(model == null) return NotFound();    
            return View(model);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            await _categoryProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
