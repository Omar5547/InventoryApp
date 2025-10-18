using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.ViewModel
{
    public class SalesOrderItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Qty { get; set; }
        public decimal Tax { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total => Qty * UnitPrice * (1 - Discount / 100) * (1 + Tax / 100);
        public List<SelectListItem> ProductList { get; set; } 
            = new List<SelectListItem>();


    }
}
