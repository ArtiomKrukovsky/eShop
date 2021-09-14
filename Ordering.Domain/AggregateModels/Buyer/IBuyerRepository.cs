using System.Threading.Tasks;
using Ordering.Domain.Seedwork;

namespace Ordering.Domain.AggregateModels.Buyer
{
    public interface IBuyerRepository : IRepository<Buyer>
    {
        Buyer Add(Buyer buyer);
        Buyer Update(Buyer buyer);
        Task<Buyer> FindAsync(string identity);
        Task<Buyer> FindByIdAsync(string id);
    }
}