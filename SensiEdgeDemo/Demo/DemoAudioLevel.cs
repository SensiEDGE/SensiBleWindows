using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoAudioLevel : INext<AudioLevel>
    {
        public AudioLevel Next()
        {
            return new Faker<AudioLevel>()
                .RuleFor(x => x.Level, f => f.Random.Byte(0, 100))
                .Generate();
        }
    }
}