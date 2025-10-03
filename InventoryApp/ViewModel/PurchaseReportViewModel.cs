namespace InventoryApp.ViewModel
{
    public class PurchaseReportViewModel
    {
        public int OrderId { get; set; }
        public string? VendorName { get; set; }
        public decimal Discount { get; set; } 
        public string? Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }


    }
}
