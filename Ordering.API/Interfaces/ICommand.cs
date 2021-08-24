using MediatR;

namespace Ordering.API.Interfaces
{
    public interface ICommand<out T> : IRequest<T>
    {
    }

    public interface ICommand : IRequest
    {
    }
}