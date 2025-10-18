using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
       
        public string? Description { get; set; }
        
        public decimal Quantity { get; set; }

       
        public decimal SalePrice { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? CategoryId { get; set; }
        
        public string ?CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();
        }
    }
