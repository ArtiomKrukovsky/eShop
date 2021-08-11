using Ordering.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDTO> GetOrderAsync(int id);

        Task<IEnumerable<OrderSummaryDTO>> GetOrdersAsync();
    }
}
