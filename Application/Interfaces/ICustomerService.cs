using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
         Task<IReadOnlyList<CustomerDto>> GetAllAsync(string? search =null);
        Task<CustomerDto?> GetByIdAsync(int id);
        Task <int> AddAsync(CustomerDto customerDto);
        Task UpdateAsync(CustomerDto customerDto);
        Task DeleteAsync(int id);

    }
}
