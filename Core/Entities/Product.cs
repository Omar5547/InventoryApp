using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Entities
{
    public class Product :BaseEntity
    {
        public string? Code { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Quantity { get; set; }
       
        public UnitType Unit { get; set; } = UnitType.Piece;
      
        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }
        
        public Category? Category { get; set; }
        public ICollection<PurchOrderItem> PurchaseOrderItems { get; set; } = new List<PurchOrderItem>();
        public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    }
}
