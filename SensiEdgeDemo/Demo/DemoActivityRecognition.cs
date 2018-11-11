using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoActivityRecognition : INext<ActivityRecognition>
    {
        public ActivityRecognition Next()
        {
            return new Faker<ActivityRecognition>()
                .RuleFor(x => x.Activity, (f, x) => f.Random.Enum<ActivityType>())
                .Generate();
        }
    }
}