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
        Task UpdateAsync(SalesOrderDto salesOrderDto);
        Task AddItemAsync(SalesOrderItemDto item);
        Task UpdateItemAsync(SalesOrderItemDto item);
        Task RemoveItemAsync(int itemId);
        Task<SalesOrderDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<SalesOrderListItemDto>> ListAsync(string? search = null);
        Task PostAsync(int orderId, int locationId);
        Task CancelAsync(int orderId);
        Task DeleteAsync(int id);

    }
}
