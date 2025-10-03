using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerProvider _customerProvider;

        public CustomerController(CustomerProvider customerProvider)
        {
            _customerProvider = customerProvider;
        }
        // GET: CustomerController
        public async Task<IActionResult> Index(string search)
        {
            var Customer = await _customerProvider.GetAllAsync(search);
            return View(Customer);
        }

        // GET: CustomerController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerProvider.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _customerProvider.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: CustomerController/Edit/5
        public async Task <IActionResult> Edit(int id)
        {
            var model = await _customerProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _customerProvider.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
         
        }

        // GET: CustomerController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _customerProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            await _customerProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
