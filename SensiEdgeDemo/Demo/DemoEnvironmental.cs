using Bogus;
using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdgeDemo.Demo
{
    public class DemoEnvironmental : INext<Environmental>
    {
        public Environmental Next()
        {
            return new Faker<Environmental>()
                .RuleFor(x => x.Humidity, f => f.Random.Short(0, 1000))
                .RuleFor(x => x.Pressure, f => f.Random.Int(900000, 1100000))
                .RuleFor(x => x.Temperature, f => f.Random.Short(0, 400))
                .Generate();
        }
    }
}
