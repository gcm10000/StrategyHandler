using System.Threading;
using System.Threading.Tasks;

namespace StrategyHandler.Tests.HandlerModels
{
    public class C : IStrategyWithKey<DTO, EKeys, string>
    {
        private readonly IAnyInterface _objectClassA;

        public C(IAnyInterface objectClass)
        {
            _objectClassA = objectClass;
        }


        public EKeys Key => EKeys.C;

        public async Task<string> Execute(DTO request, CancellationToken cancellationToken = default)
        {
            return (await _objectClassA.Execute()).ToString();
        }
    }
}
