using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StrategyHandler.DependencyInjection
{
    public static class StrategyHandlerExtensions
    {
        private static object _handlerContextBuilder = default;

        public static IServiceCollection RegisterStrategyHandlerAsSingleton<TKey>(
            this IServiceCollection services)
        {
            services.AddSingleton(BuildHandlerContext<TKey>);
            return services;
        }

        public static IServiceCollection RegisterStrategyHandlerAsScoped<TKey>(
            this IServiceCollection services)
        {
            services.AddSingleton(BuildHandlerContext<TKey>);
            return services;
        }

        public static IServiceCollection RegisterStrategyHandlerAsTransient<TKey>(
            this IServiceCollection services)
        {
            services.AddTransient(BuildHandlerContext<TKey>);
            return services;
        }

        private static HandlerContext<TKey> BuildHandlerContext<TKey>(
            IServiceProvider serviceProvider)
        {
            if (_handlerContextBuilder is null)
                _handlerContextBuilder = new HandlerContextBuilder<TKey>();

            using (var handlerContextBuilder = _handlerContextBuilder as HandlerContextBuilder<TKey>)
            {
                var strategyServices = serviceProvider.GetServices(typeof(IStrategy));
                var strategies = GetStrategies<TKey>(strategyServices);
                var handlerContext = handlerContextBuilder.Build(strategies);
                return handlerContext;
            }
        }

        static IEnumerable<(TKey key, IStrategy strategyImplmentation)> GetStrategies<TKey>(
            IEnumerable<object> strategyServices)
        {
            var property = nameof(IStrategyWithKey<DTO, DTO, object>.Key);
            foreach (var strategyService in strategyServices)
            {
                var value = (TKey)strategyService.GetPropValue(property);
                yield return (value, (IStrategy)strategyService);
            }
        }

        public static IServiceCollection AddComponentInStrategyHandler<TKey>(
            this IServiceCollection services, TKey key, IStrategy implementation)
        {
            if (_handlerContextBuilder is null)
            {
                _handlerContextBuilder = new HandlerContextBuilder<TKey>();
            }
            
            (_handlerContextBuilder as HandlerContextBuilder<TKey>).Add(key, implementation);
            return services;
        }

        public static IServiceCollection ClearStrategyHandler<TKey>(
            this IServiceCollection services)
        {
            ((HandlerContextBuilder<TKey>)_handlerContextBuilder)?.Clear();
            return services;
        }
        
        public static IServiceCollection AddComponentsInStrategyHandlerByAssembly<TKey>(
            this IServiceCollection services, Assembly assembly)
        {
            if (_handlerContextBuilder is null)
            {
                _handlerContextBuilder = new HandlerContextBuilder<TKey>();
            }

            var interfaceType = typeof(IStrategyWithKey<,,>);

            var types = assembly.GetTypes()
                .Where(p => p.IsClass && p.GetInterface(interfaceType.Name) != null);

            foreach (var type in types)
                services.AddScoped(typeof(IStrategy), type);

            return services;
        }

        static object GetPropValue(this object @object, string propName)
        {
            return @object.GetType().GetProperty(propName).GetValue(@object, null);
        }
    }
}
