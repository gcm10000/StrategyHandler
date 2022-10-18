using System.Threading;
using System.Threading.Tasks;

namespace StrategyHandler
{
    public interface IStrategy<in TRequest, TResponse> : IStrategy
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
    }

    public interface IStrategy
    {

    }
}
