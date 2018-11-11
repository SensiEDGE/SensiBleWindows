using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoOrientation : INext<Orientation>
    {
        public Orientation Next()
        {
            var quad = new Faker<Vector3D>()
                .RuleFor(v => v.X, f => f.Random.Short(0, 100))
                .RuleFor(v => v.Y, f => f.Random.Short(0, 100))
                .RuleFor(v => v.Z, f => f.Random.Short(0, 100))
                .Generate();

            return new Faker<Orientation>()
                 .RuleFor(x => x.Quat0, f => quad)
                 .RuleFor(x => x.Quat1, f => quad)
                 .RuleFor(x => x.Quat2, f => quad)
                 .Generate();
        }
    }
}