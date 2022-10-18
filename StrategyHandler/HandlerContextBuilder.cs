using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyHandler
{
    public class HandlerContextBuilder<TKey> : IDisposable
    {
        private readonly List<(TKey Key, object StrategyImplementation)> _objectsTuple;
        private bool _disposed;

        public HandlerContextBuilder()
        {
            _objectsTuple = new List<(TKey, object)>();
        }

        public void Add(TKey key, IStrategy strategy)
        {
            _objectsTuple.Add((key, strategy));
        }

        public void Add(TKey key, object strategyImplementation)
        {
            _objectsTuple.Add((key, strategyImplementation));
        }

        public void Clear()
        {
            _objectsTuple.Clear();
        }

        public void Remove(TKey key)
        {
            var @object = _objectsTuple.FirstOrDefault(x => x.Key.Equals(key));
            _objectsTuple.Remove(@object);
        }

        public HandlerContext<TKey> Build(object @object = null)
        {
            var handler = new HandlerContext<TKey>();
            foreach (var (Key, StrategyImplementation) in _objectsTuple)
                handler.Add(Key, StrategyImplementation);

            return handler;
        }

        public HandlerContext<TKey> Build(IEnumerable<(
            TKey key, IStrategy strategyImplmentation)> strategiesImplementation)
        {
            var handler = new HandlerContext<TKey>();
            foreach (var (Key, StrategyImplementation) in _objectsTuple)
                handler.Add(Key, StrategyImplementation);

            foreach (var (key, strategyImplmentation) in strategiesImplementation)
                handler.Add(key, strategyImplmentation);

            return handler;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _objectsTuple.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
