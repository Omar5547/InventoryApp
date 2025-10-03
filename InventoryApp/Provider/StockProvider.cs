using Application.Interfaces;
using InventoryApp.ViewModel;

namespace InventoryApp.Provider
{
    public class StockProvider
    {
        private readonly IStockService _stockService;

        public StockProvider( IStockService stockService)
        {
            _stockService = stockService;
        }
        public async Task <List<StockViewModel>> GetAllStockAsync(string? search = null)
        {
            var dtos = await _stockService.GetAllStockAsync();
            var stocks = dtos.Select(d => new StockViewModel
            {
                productId = d.ProductId,
                productName = d.ProductName,
                locationId = d.LocationId,
                locationName = d.LocationName,
                Qty = d.Qty,
                
            });
            if (!string.IsNullOrEmpty(search))
            {
               
                stocks = stocks.Where(s => s.productName.Contains(search) || s.locationName.Contains(search));
            }
            return stocks.ToList();

        }
        public async Task <StockIndexViewModel> GetStockByProductAsync (int productId , string? search = null)
        {
            var dtos = await _stockService.GetAllStockAsync();
            var filteredDtos = dtos.Where(d => d.ProductId == productId);
            var stocks = filteredDtos.Select(d => new StockViewModel
            {
                productId = d.ProductId,
                productName = d.ProductName,
                locationId = d.LocationId,
                locationName = d.LocationName,
                Qty = d.Qty,
            });
            if (!string.IsNullOrEmpty(search))
            {
                stocks = stocks.Where(s => s.locationName.Contains(search));
            }
            var viewModel = new StockIndexViewModel
            {
                Title = "Stock List",
                Search = search,
                stocks = stocks.ToList()
            };
            return viewModel;

        }
        public async Task<StockIndexViewModel> GetStockByLocationAsync(int locationId, string? search = null)
        {
            var dtos = await _stockService.GetAllStockAsync();
            var filteredDtos = dtos.Where(d => d.LocationId == locationId);
            var stocks = filteredDtos.Select(d => new StockViewModel
            {
                productId = d.ProductId,
                productName = d.ProductName,
                locationId = d.LocationId,
                locationName = d.LocationName,
                Qty = d.Qty,
            });
            if (!string.IsNullOrEmpty(search))
            {
                stocks = stocks.Where(s => s.productName.Contains(search));
            }
            var viewModel = new StockIndexViewModel
            {
                Title = "Stock List",
                Search = search,
                stocks = stocks.ToList()
            };
            return viewModel;
        }
    }
}
