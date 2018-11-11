using Bogus;
using SensiEdge.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdgeDemo.Demo
{
    public class DemoUltraViolet : INext<UltraViolet>
    {
        public UltraViolet Next()
        {
            return new Faker<UltraViolet>()
                .RuleFor(x => x.Value, f => f.Random.UShort(0, 1000))
                .Generate();
        }
    }
}
