using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductListItemDto>> GetAllAsync(string? search = null, int? skip = null, int? take = null);
        Task<ProductDto?> GetByIdAsync(int id);
        Task<int> AddAsync(ProductDto productDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsSkuAsync(string sku, int? excludeId = null);
    }
}
