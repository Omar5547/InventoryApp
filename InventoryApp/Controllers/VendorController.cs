using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class VendorController : Controller
    {
        private readonly VendorProvider _vendorProvider;

        public VendorController(VendorProvider vendorProvider)
        {
            _vendorProvider = vendorProvider;
        }
        // GET: SupplierController
        public async Task<IActionResult> Index(string search)
        {
            var vendors = await _vendorProvider.GetAllAsync(search);
            return View(vendors);
        }

        // GET: SupplierController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vendors = await _vendorProvider.GetByIdAsync(id);
            if (vendors == null) return NotFound();

            return View(vendors);
        }

        // GET: SupplierController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VendorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _vendorProvider.CreateAsync(model);
            return RedirectToAction(nameof(Index));
                
        }

        // GET: SupplierController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _vendorProvider.GetByIdAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        // POST: SupplierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VendorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _vendorProvider.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: SupplierController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _vendorProvider.GetByIdAsync(id);
            if(model == null) return NotFound();

            return View(model);
        }

        // POST: SupplierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            await _vendorProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
