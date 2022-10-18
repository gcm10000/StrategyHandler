using System.Threading;
using System.Threading.Tasks;

namespace StrategyHandler.Tests.HandlerModels
{
    public class B : IStrategy<DTO, string>
    {
        public async Task<string> Execute(DTO request, CancellationToken cancellationToken = default)
        {
            return StringConstants.TestDefaultSuccess;
        }
    }
}
