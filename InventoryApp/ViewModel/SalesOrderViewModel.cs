using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.ViewModel
{
    public class SalesOrderViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public List<SelectListItem> CustomerList { get; set; } = new();
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal Discount { get; set; }
        public string Currency { get; set; } = "USD";
        public OrderStatus Status { get; set; } 
        public List<SalesOrderItemViewModel> Items { get; set; } = new List<SalesOrderItemViewModel>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<ProductViewModel> ProductList { get; set; } = new List<ProductViewModel>();

    }
}
