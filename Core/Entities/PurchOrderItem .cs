using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PurchOrderItem :BaseEntity
    {
        public int PurchOrderId{ get; set; }
       
        public PurchaseOrder? PurchOrder { get; set; } 

        
        public int ProductId { get; set; }
        
        public Product? Product { get; set; } 
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
