using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVendorService
    {
        Task<IReadOnlyList<VendorDto>> GetAllAsync(string? search=null);
        Task<VendorDto?> GetByIdAsync(int id);
        Task <int> AddAsync(VendorDto vendorDto);
        Task UpdateAsync(VendorDto vendorDto);
        Task DeleteAsync(int id);
    }
}
