namespace InventoryApp.ViewModel
{
    public class SalesReportViewModel
    {
        public int OrderId { get; set; }
        
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
