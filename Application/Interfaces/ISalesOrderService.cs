using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISalesOrderService
    {
        Task<int> AddAsync(SalesOrderDto salesOrderDto, IEnumerable<SalesOrderItemDto> items);
        Task UpdateAsync(SalesOrderDto salesOrderDto, IEnumerable<SalesOrderItemDto> items);
        Task<SalesOrderDto?> GetByIdAsync(int id);
        Task<IEnumerable<SalesOrderListItemDto>> GetAllAsync(string? search = null);
        Task DeleteAsync(int id);

    }
}
