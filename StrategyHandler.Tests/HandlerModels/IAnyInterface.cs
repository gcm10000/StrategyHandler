using System.Threading.Tasks;

namespace StrategyHandler.Tests.HandlerModels
{
    public interface IAnyInterface
    {
        public Task<bool> Execute();
    }
}