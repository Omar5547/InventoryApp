using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class VendorDto : BaseDto
    {
        
        public string Name { get; set; } = string.Empty;
        public string? Phone {  get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

    }
}
