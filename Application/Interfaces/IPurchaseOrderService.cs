using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<IReadOnlyList<PurchaseOrderListItemDto>> GetAllAsync(string? search = null);
        Task<PurchaseOrderDto?> GetByIdAsync(int id);
        Task <int> AddAsync(PurchaseOrderDto orderDto , IEnumerable<PurchOrderItemDto> items);

        Task UpdateAsync(PurchaseOrderDto orderDto);
        Task AddOrUpdateItemAsync(PurchOrderItemDto item);
        Task RemoveItemAsync(int purchOrderId , int productId);
        Task PostAsync(int orderId , int locationId);
        Task CancelAsync(int orderId);
        Task DeleteAsync(int id);
    }
}
