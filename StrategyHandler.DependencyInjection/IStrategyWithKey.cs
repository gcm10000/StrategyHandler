namespace StrategyHandler
{
    /// <summary>
    /// Use this interface to work with automatic insertion in the container.
    /// </summary>
    /// <typeparam name="TRequest">The request to process.</typeparam>
    /// <typeparam name="TKey">The key to execute the method of class.</typeparam>
    /// <typeparam name="TResponse">The return.</typeparam>
    public interface IStrategyWithKey<in TRequest, TKey, TResponse>
        : IStrategy<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        TKey Key { get; }
    }
}
