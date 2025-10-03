using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Category : BaseEntity
    {
      
        public string Name { get; set; } = null!;
        public string ?Description { get; set; }
       
        public string? productName { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
