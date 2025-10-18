using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? CategoryName {  get; set; }
        public decimal SalePrice { get; set; }
        public UnitType Unit { get; set; }
        public bool IsActive { get; set; }
    }
}
