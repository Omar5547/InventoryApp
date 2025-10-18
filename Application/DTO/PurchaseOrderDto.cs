using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PurchaseOrderDto : BaseDto
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
       
        
        public string Currency { get; set; } = "USD";
        public OrderStatus Status { get; set; } = OrderStatus.Open;
        public List<PurchOrderItemDto> Items { get; set; } = new List<PurchOrderItemDto>();
    }
}
