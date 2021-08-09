using Ordering.Domain.Seedwork;
using System.Threading.Tasks;

namespace Ordering.Domain.AggregateModels.Ordering
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task AddAsync(Order order);

        void Update(Order order);

        Task<Order> GetAsync(int orderId);
    }
}
