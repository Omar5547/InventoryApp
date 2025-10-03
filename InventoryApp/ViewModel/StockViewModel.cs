namespace InventoryApp.ViewModel
{
    public class StockViewModel
    {
        public int productId { get; set; }
        public string productName { get; set; } = string.Empty;
        public int locationId { get; set; }
        public string locationName { get; set; } = string.Empty;
        public decimal Qty { get; set; }
    }
}
