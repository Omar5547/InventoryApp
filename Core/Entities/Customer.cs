using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Customer : BaseEntity
    {
      
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Fax { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public decimal? Discount { get; set; }
        public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    }
}
