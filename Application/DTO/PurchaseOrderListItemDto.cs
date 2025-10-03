using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PurchaseOrderListItemDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public string Currency {  get; set; } = string.Empty;
        public OrderStatus Status {  get; set; }
        public decimal Discount { get; set; }
        public decimal NonVendorCosts { get; set; }
    }
}
