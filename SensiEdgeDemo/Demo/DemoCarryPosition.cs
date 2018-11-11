using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoCarryPosition : INext<CarryPosition>
    {
        public CarryPosition Next()
        {
            return new Faker<CarryPosition>()
                .RuleFor(x => x.Position, f => f.Random.Enum<CarryType>())
                .Generate();
        }
    }
}