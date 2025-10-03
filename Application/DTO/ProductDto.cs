using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ProductDto : BaseDto
    {
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }= string.Empty;
    }
}
