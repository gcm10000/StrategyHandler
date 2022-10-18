using StrategyHandler.Tests.HandlerModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StrategyHandler.Tests
{
    public class UnitTestStrategyHandler
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { EKeys.A, new A() },
                new object[] { EKeys.B, new B() },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public async void ShouldReturnMessageTestWhenSuccessful(EKeys key, object @object)
        {
            var handlerContextBuilder = new HandlerContextBuilder<EKeys>();
            handlerContextBuilder.Add(key, @object);
            var handlerContext = handlerContextBuilder.Build();

            var result = await handlerContext.Execute<string>(key, @object);
            Assert.Equal(StringConstants.TestDefaultSuccess, result);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async void ShouldThrowsExceptionWhenKeyNotFound(EKeys key, object @object)
        {
            var handlerContextBuilder = new HandlerContextBuilder<EKeys>();

            var handlerContext = handlerContextBuilder.Build();

            async Task action() => await handlerContext.Execute<string>(key, @object);

            await Assert.ThrowsAsync<KeyNotFoundException>(action);
        }
    }
}