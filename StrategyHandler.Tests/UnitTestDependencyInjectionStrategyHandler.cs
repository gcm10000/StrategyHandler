using Microsoft.Extensions.DependencyInjection;
using StrategyHandler.DependencyInjection;
using StrategyHandler.Tests.HandlerModels;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace StrategyHandler.Tests
{
    public class UnitTestDependencyInjectionStrategyHandler
    {
        public UnitTestDependencyInjectionStrategyHandler()
        {

        }

        [Fact]
        public async Task ShouldReturnSuccessWhenExecuteMethodUsingDependencyInjectionAddingAsSingleton()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            services.AddComponentInStrategyHandler(EKeys.A, new A());
            services.RegisterStrategyHandlerAsSingleton<EKeys>();
            var result = await GetPartialActAsync(services);

            //Assert
            Assert.Equal(StringConstants.TestDefaultSuccess, result);
        }

        [Fact]
        public async Task ShouldReturnSuccessWhenExecuteMethodUsingDependencyInjectionAddingAsScoped()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            services.AddComponentInStrategyHandler(EKeys.A, new A());
            services.RegisterStrategyHandlerAsScoped<EKeys>();
            var result = await GetPartialActAsync(services);

            //Assert
            Assert.Equal(StringConstants.TestDefaultSuccess, result);
        }

        [Fact]
        public async Task ShouldReturnSuccessWhenExecuteMethodUsingDependencyInjectionAddingAsTransient()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            services.AddComponentInStrategyHandler(EKeys.A, new A());
            services.RegisterStrategyHandlerAsTransient<EKeys>();
            var result = await GetPartialActAsync(services);

            //Assert
            Assert.Equal(StringConstants.TestDefaultSuccess, result);
        }

        [Fact]
        public async Task ShouldReturnSuccessWhenAddingClassesUsingAssemblyAndDependencyInjection()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            var assembly = Assembly.GetAssembly(typeof(A));
            services.AddComponentsInStrategyHandlerByAssembly<EKeys>(assembly);
            services.RegisterStrategyHandlerAsSingleton<EKeys>();

            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetService<HandlerContext<EKeys>>();

            if (context is null)
                throw new NullReferenceException();

            var result = await context.Execute(EKeys.A, new DTO());

            //Assert
            Assert.Equal(StringConstants.TestDefaultSuccess, result);
        }

        [Fact]
        public async Task ShouldReturnFailedWhenAddingSameClassesUsingManualProccessAndAssembly()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            static async Task GetAct(IServiceCollection services)
            {
                services.AddComponentInStrategyHandler(EKeys.A, new A());
                var assembly = Assembly.GetAssembly(typeof(A));
                services.AddComponentsInStrategyHandlerByAssembly<EKeys>(assembly);
                services.RegisterStrategyHandlerAsSingleton<EKeys>();

                var serviceProvider = services.BuildServiceProvider();
                var context = serviceProvider.GetService<HandlerContext<EKeys>>();

                if (context is null)
                    throw new NullReferenceException();

                var result = await context.Execute(EKeys.A, new DTO());
            }

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() => GetAct(services));
        }

        [Fact]
        public async Task ShouldReturnSuccessWhenAddingManuallyClassAndAutomaticallyAssembly()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            services.AddComponentInStrategyHandler(EKeys.B, new B());
            var assembly = Assembly.GetAssembly(typeof(A));
            services.AddComponentsInStrategyHandlerByAssembly<EKeys>(assembly);
            services.RegisterStrategyHandlerAsSingleton<EKeys>();

            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetService<HandlerContext<EKeys>>();

            if (context is null)
                throw new NullReferenceException();

            var resultA = await context.Execute(EKeys.A, new DTO());
            var resultB = await context.Execute(EKeys.B, new DTO());

            //Assert
            Assert.Equal(StringConstants.TestDefaultSuccess, resultA);
            Assert.Equal(StringConstants.TestDefaultSuccess, resultB);
        }

        [Fact]
        public async Task ShouldReturnFailedWhenExecuteMethodUsingDependencyInjectionWithoutRegister()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            services.AddComponentInStrategyHandler(EKeys.A, new A());
            var partialAct = () => GetPartialActAsync(services);

            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(partialAct);
        }

        static async Task<string> GetPartialActAsync(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetService<HandlerContext<EKeys>>();

            if (context is null)
            {
                services.ClearStrategyHandler<EKeys>();
                throw new NullReferenceException();
            }

            var result = await context.Execute(EKeys.A, new DTO());
            return result;
        }
    }
}
