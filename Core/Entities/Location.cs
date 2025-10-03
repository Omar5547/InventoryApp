using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public ICollection<ProductStock> Stocks { get; set; } =  new List<ProductStock>();
    }
}
