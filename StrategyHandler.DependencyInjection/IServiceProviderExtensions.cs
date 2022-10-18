using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace StrategyHandler.DependencyInjection
{
    public static class IServiceProviderExtensions
    {

        public static IEnumerable<object> GetServices(
            this IServiceProvider provider, Type serviceType)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)provider.GetRequiredService(genericEnumerable);
        }
    }
}
