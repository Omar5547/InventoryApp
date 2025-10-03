using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Vendor : BaseEntity
    {
        
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrder { get; set; } = new List<PurchaseOrder>();
    }
}
