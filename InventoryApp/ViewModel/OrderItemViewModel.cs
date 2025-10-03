using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.ViewModel
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<SelectListItem> ProductList { get; set; } = new List<SelectListItem>();
    }
}
