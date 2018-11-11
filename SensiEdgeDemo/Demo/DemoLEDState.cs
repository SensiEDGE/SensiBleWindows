using Bogus;
using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoLEDState : INext<LEDState>
    {
        public LEDState Next()
        {
            return new Faker<LEDState>()
                .RuleFor(x => x.State, f => f.Random.Enum<LEDStateType>())
                .Generate();                    
        }
    }
}