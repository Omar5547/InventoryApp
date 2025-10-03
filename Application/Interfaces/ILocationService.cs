using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILocationService
    {
        Task<int> AddAsync(LocationDto locationDto);
        Task<IReadOnlyList<LocationDto>> GetAllAsync(string? search = null);
        Task<LocationDto?> GetByIdAsync(int id);
        Task UpdateAsync( LocationDto locationDto);
        Task DeleteAsync(int id);
    }
}
