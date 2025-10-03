using InventoryApp.Provider;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly SalesOrderProvider _salesOrderProvider;

        public SalesOrderController(SalesOrderProvider salesOrderProvider)
        {
            _salesOrderProvider = salesOrderProvider;
        }
        // GET: SalesOrderController
        public async Task <IActionResult> Index(string search)
        {
            var salesorders = await _salesOrderProvider.GetAllAsync(search);
            return View(salesorders);
        }

        // GET: SalesOrderController/Details/5
        public async Task <IActionResult> Details(int id)
        {
            var salesorder = await _salesOrderProvider.GetByIdAsync(id);
            if (salesorder == null)
            {
                return NotFound();
            }
            return View(salesorder);
        }

        // GET: SalesOrderController/Create
        [HttpGet]
        public async Task <IActionResult> Create()
        {
            var salesorder = await _salesOrderProvider.GetEmptyWithCustomerAsync();
            return View(salesorder);
        }

        // POST: SalesOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesOrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            await _salesOrderProvider.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: SalesOrderController/Edit/5
        public async Task <IActionResult> Edit(int id)
        {
            var salesorder = await _salesOrderProvider.GetByIdAsync(id);
            if (salesorder == null)
            {
                return NotFound();
            }
            return View(salesorder);
        }

        // POST: SalesOrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(SalesOrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            await _salesOrderProvider.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: SalesOrderController/Delete/5
        [HttpGet]
        public async Task <IActionResult> Delete(int id)
        {
            var salesorder = await _salesOrderProvider.GetByIdAsync(id);
            if (salesorder == null)
            {
                return NotFound();
            }
            return View(salesorder);
        }

        // POST: SalesOrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _salesOrderProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Post(int id, int locationId)
        {
            await _salesOrderProvider.PostAsync(id, locationId);
            return RedirectToAction(nameof(Details), new { id });
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            await _salesOrderProvider.CancelAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
