using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductStock : BaseEntity
    {
        public int ProductId { get; set; }
        
        public Product? Product { get; set; }
        public int LocationId { get; set; }
       
        public Location? Location { get; set; }
        public decimal Qty { get; set; }
     
    }
}
