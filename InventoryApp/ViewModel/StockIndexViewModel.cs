namespace InventoryApp.ViewModel
{
    public class StockIndexViewModel
    {
        public string Title { get; set; } = "Stock List";   
        public string? Search { get; set; }
        public List<StockViewModel> stocks { get; set; } = new List<StockViewModel>();
    }
}
