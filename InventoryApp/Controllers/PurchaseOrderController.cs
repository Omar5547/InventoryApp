using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly PurchaseOrderProvider _orderProvider;

        public PurchaseOrderController(PurchaseOrderProvider orderProvider)
        {
            _orderProvider = orderProvider;
        }
        // GET: OrderController
        public async Task<IActionResult> Index(string search)
        {
            var order = await _orderProvider.GetAllAsync();

            return View(order);
        }

        // GET: OrderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _orderProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // GET: OrderController/Create
        public async Task<IActionResult> Create()
        {
            var model = await _orderProvider.GetEmptyWithVendorsAsync();
            return View(model);
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( PurchaseOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _orderProvider.CreateAsync(model);
                return Ok();
            }
            else
            {
                var salesorder = await _orderProvider.GetEmptyWithVendorsAsync();
                return BadRequest(salesorder);
            }
        }
        // GET: OrderController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _orderProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOrderViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _orderProvider.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }


        // GET: OrderController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _orderProvider.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            await _orderProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
