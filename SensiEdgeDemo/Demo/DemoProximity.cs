using Bogus;
using SensiEdge.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdgeDemo.Demo
{
    public class DemoProximity : INext<Proximity>
    {
        public Proximity Next()
        {
            return new Faker<Proximity>()
                .RuleFor(x => x.Distance, f => f.Random.UShort(0, 1000))
                .Generate();
        }
    }
}
