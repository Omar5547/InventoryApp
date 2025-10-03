using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public int VendorId { get; set; }
        public Vendor? Vendor { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal Discount { get; set; }
        public decimal NonVendorCosts { get; set; }
        public string Currency { get; set; } = "USD";
        public OrderStatus Status { get; set; } = OrderStatus.Open;
        public ICollection<PurchOrderItem> Items { get; set; } = new List<PurchOrderItem>();
    }
}
