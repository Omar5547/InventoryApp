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
        Task<IEnumerable<PurchaseOrderListItemDto>> GetAllAsync(string? search = null);
        Task<PurchaseOrderDto?> GetByIdAsync(int id);
        Task <int> AddAsync(PurchaseOrderDto orderDto , IEnumerable<PurchOrderItemDto> items);
        Task UpdateAsync(PurchaseOrderDto orderDto, IEnumerable<PurchOrderItemDto> items);
        Task DeleteAsync(int id);
    }
}
