using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task<List<OrderReportDto>> GetOrderReportsAsync(DateTime? startDate = null , DateTime?endDate=null);

        Task<List<SalesReportDto>> GetSalesReportsAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
