using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class SalesOrderDto : BaseDto
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string CustomerName { get; set; } = string.Empty;
        public string Currency { get; set; } = "USD";
        public OrderStatus Status { get; set; } = OrderStatus.Open;
        public List<SalesOrderItemDto> Items { get; set; } = new List<SalesOrderItemDto>();
    }
}
