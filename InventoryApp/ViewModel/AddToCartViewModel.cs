using System.ComponentModel.DataAnnotations;

namespace InventoryApp.ViewModel
{
    public class AddToCartViewModel
    {
        [Required]
        public int ProductId { get; set; }
        [Range(1, 100, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;
        public string? ReturnUrl { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
