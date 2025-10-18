using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PurchOrderItemDto : BaseDto
    {
        public int PurchOrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
