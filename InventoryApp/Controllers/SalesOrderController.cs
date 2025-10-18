using Core.Entities;
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
        public async Task<IActionResult> Create( SalesOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _salesOrderProvider.CreateAsync(model);
                return Ok();

            }
          

            var salesorder = await _salesOrderProvider.GetEmptyWithCustomerAsync();
            return BadRequest(new {  salesorder });
        }

        // GET: SalesOrderController/Edit/5
        public async Task <IActionResult> Edit(int id)
        {
            var salesorder = await _salesOrderProvider.GetByIdAsync(id);
            if (salesorder == null)
            {
                return NotFound();
            }
            if (salesorder.ProductList == null || salesorder.ProductList.Count == 0)
            {
                var emptyModel = await _salesOrderProvider.GetEmptyWithCustomerAsync();
                salesorder.ProductList = emptyModel.ProductList;
            }
            return View(salesorder);
        }

        // POST: SalesOrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( SalesOrderViewModel model)
        {
            if (ModelState.IsValid)
            { 
                await _salesOrderProvider.UpdateAsync(model);
                return Ok();
            }
            var emptyModel = await _salesOrderProvider.GetEmptyWithCustomerAsync();
            model.ProductList = emptyModel.ProductList;
            model.CustomerList = emptyModel.CustomerList;
            return BadRequest(model);
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
        public async Task<IActionResult> Delete(int id , IFormCollection collection)
        {
            await _salesOrderProvider.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
      
    }
}
