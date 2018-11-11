using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoAccGyroMag : INext<AccGyroMag>
    {
        ushort time = 0;
        public AccGyroMag Next()
        {
            var quad = new Faker<Vector3D>()
                .RuleFor(v => v.X, f => f.Random.Short(0, 100))
                .RuleFor(v => v.Y, f => f.Random.Short(0, 100))
                .RuleFor(v => v.Z, f => f.Random.Short(0, 100))
                .Generate();
            time += 100;
            return new Faker<AccGyroMag>()
                 .RuleFor(x => x.TimeStamp, time)
                 .RuleFor(x => x.Acc, f => quad)
                 .RuleFor(x => x.Gyro, f => quad)
                 .RuleFor(x => x.Mag, f => quad)
                 .Generate();
        }
    }
}