using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoCompass : INext<Compass>
    {
        public Compass Next()
        {
            return new Faker<Compass>()
                .RuleFor(x => x.Angle, f => f.Random.UShort(0, 36000))
                .Generate();
        }
    }
}