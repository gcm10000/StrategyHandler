using System.Threading;
using System.Threading.Tasks;

namespace StrategyHandler.Tests.HandlerModels
{
    public class A : IStrategyWithKey<DTO, EKeys, string>
    {
        public EKeys Key => EKeys.A;

        public async Task<string> Execute(DTO request, CancellationToken cancellationToken = default)
        {
            return StringConstants.TestDefaultSuccess;
        }
    }
}
