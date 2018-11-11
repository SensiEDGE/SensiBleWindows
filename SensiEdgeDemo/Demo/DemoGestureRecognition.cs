using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoGestureRecognition : INext<GestureRecognition>
    {
        public GestureRecognition Next()
        {
            return new Faker<GestureRecognition>()
                .RuleFor(x => x.Gesture, f => f.Random.Enum<GestureType>())
                .Generate();
        }
    }
}