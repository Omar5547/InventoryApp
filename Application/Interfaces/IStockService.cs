using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStockService
    {
        Task AdjustStockAsync (int productId, int locationId , decimal deltaQty, string sourceType , int? sourceId);
        Task<decimal> GetAvailableStockAsync(int productId, int locationId);
        Task<List<ProductStockDto>> GetAllStockAsync();
    }
}
