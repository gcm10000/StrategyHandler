using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyHandler
{
    public class HandlerContext<TKey>
    {
        private readonly Dictionary<TKey, IStrategy> _dictionary;

        internal HandlerContext()
        {
            _dictionary = new Dictionary<TKey, IStrategy>();
        }

        internal void Add(TKey key, IStrategy implementation)
            => _dictionary.Add(key, implementation);
        
        internal void Add(TKey key, object implementation)
        {
            IStrategy strategy = implementation as IStrategy;
            Add(key, strategy);
        }

        public async Task<TResponse> Execute<TResponse>(
            TKey key, object request, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            var requestParsed = request as IRequest<TResponse>;
            return await Execute(key, requestParsed, cancellationToken);
        }

        public async Task<TResponse> Execute<TResponse>(
            TKey key, IRequest<TResponse> request, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            var strategy = _dictionary[key];
            var type = strategy.GetType();
            var methodName = nameof(IStrategy<IRequest<TResponse>, TResponse>.Execute);
            var method = type.GetMethod(methodName);
            var task = (Task<TResponse>)method.Invoke(strategy, new object[] { request, cancellationToken });
            return await task;
        }
    }
}
