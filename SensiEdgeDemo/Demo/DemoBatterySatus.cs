using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoBatterySatus : INext<BatterySatus>
    {
        public BatterySatus Next()
        {
            return new Faker<BatterySatus>()
                .RuleFor(x => x.Current, f => f.Random.UShort(0, 500))
                .RuleFor(x => x.SOC, f => f.Random.UShort(0, 1000))
                .RuleFor(x => x.Status, f => f.Random.Enum<BatteryStatusType>())
                .RuleFor(x => x.Voltage, f => f.Random.UShort(0, 4000))
                .Generate();
        }
    }
}