using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.ViewModel
{
    public class PurchaseOrderViewModel
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        public List<SelectListItem> VendorList { get; set; } = new List<SelectListItem>();
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        public string Currency { get; set; } = "USD";
        public OrderStatus Status { get; set; }
        

        public List<PurchaseOrderItemViewModel> Items { get; set; } = new List<PurchaseOrderItemViewModel>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal Total { get; set; }
        public List<ProductViewModel> ProductList { get; set; } = new();
    }
}
