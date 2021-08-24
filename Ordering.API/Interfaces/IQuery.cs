using MediatR;

namespace Ordering.API.Interfaces
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}