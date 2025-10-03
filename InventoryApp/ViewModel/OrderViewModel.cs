using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public decimal Total { get; set; }
        public int ItemsCount { get; set; }
        public int CustomerId { get; set; } 
        public string CustomerName { get; set; } = string.Empty;
        public List<OrderItemViewModel> Items { get; set; } = new ();
        public List<SelectListItem> CustomerList { get; set; } = new List<SelectListItem>();
    }
}
