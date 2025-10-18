using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SalesOrder : BaseEntity
    {
        public int CustomerId { get; set; }
       
        public Customer? Customer { get; set; } 
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        public string Currency { get; set; } = "USD";
        public OrderStatus Status { get; set; } = OrderStatus.Open;
        public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    }
}
