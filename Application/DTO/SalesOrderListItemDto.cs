using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class SalesOrderListItemDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Currency {  get; set; } = "USD";
        public OrderStatus Status { get; set; }
        public decimal Discount { get; set; }
    }
}
