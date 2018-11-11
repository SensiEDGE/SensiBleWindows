using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoLightSensor : INext<LightSensor>
    {
        public LightSensor Next()
        {
            return new Faker<LightSensor>()
                .RuleFor(x => x.Value, f => f.Random.UShort(0, 1000))
                .Generate();
        }
    }
}