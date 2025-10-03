using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationProvider _locationProvider;

        public LocationController(LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
        }
        // GET: LocationController
        public async Task <IActionResult> Index(string search)
        {
            var locations = await _locationProvider.GetAllAsync(search);
            return View(locations);
        }

        // GET: LocationController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var location = await _locationProvider.GetByIdAsync(id);
            if (location == null) return NotFound();
            return View(location);
        }

        // GET: LocationController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationViewModel model)
        {
           if (!ModelState.IsValid)
                await _locationProvider.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: LocationController/Edit/5
        public async Task <IActionResult> Edit(int id)
        {
            var model = await _locationProvider.GetByIdAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(int id, LocationViewModel model)
        {
           if (!ModelState.IsValid)
                return View(model);
            await _locationProvider.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: LocationController/Delete/5
        public async Task <IActionResult> Delete(int id)
        {
            var model = await _locationProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
           await _locationProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
