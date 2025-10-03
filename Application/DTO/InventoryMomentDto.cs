using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class InventoryMomentDto :BaseDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public decimal Qty { get; set; }
        public int? SourceId { get; set; }
        public string SourceType { get; set; } = null!;
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;



    }
}
