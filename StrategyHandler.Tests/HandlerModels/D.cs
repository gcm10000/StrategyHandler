using System.Threading.Tasks;

namespace StrategyHandler.Tests.HandlerModels
{
    public class D : IAnyInterface
    {
        public Task<bool> Execute()
        {
            return Task.FromResult(true);
        }
    }
}
