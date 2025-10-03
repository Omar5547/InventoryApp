using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(string? search=null);
        Task<CategoryDto?> GetByIdAsync(int id);
        Task <int> AddAsync (CategoryDto categoryDto);
        Task UpdateAsync( CategoryDto categoryDto);
        Task DeleteAsync(int id);
    }
}
