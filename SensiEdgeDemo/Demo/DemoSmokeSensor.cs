using Bogus;
using SensiEdge.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdgeDemo.Demo
{
    internal class DemoSmokeSensor : INext<SmokeSensor>
    {
        ushort time = 0;
        ushort timeA = 0;
        ushort timeB = 0;
        public SmokeSensor Next()
        {
            time += new Faker().Random.UShort(0, 10);
            timeA += new Faker().Random.UShort(0, 10);
            timeB += new Faker().Random.UShort(0, 10);
            return new Faker<SmokeSensor>()
                .RuleFor(x => x.TimeStamp, time)
                .RuleFor(x => x.TimeSlotA, timeA)
                .RuleFor(x => x.TimeSlotB, timeB)
                .RuleFor(x => x.Smoke, f => f.PickRandom<SmokeStateType>())
                .Generate();
        }
    }
}
