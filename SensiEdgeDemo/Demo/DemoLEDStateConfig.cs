using SensiEdge.Data;

namespace SensiEdgeDemo.Demo
{
    internal class DemoLEDStateConfig : INext<LEDStateConfig>
    {
        public LEDStateConfig Next()
        {
            return new LEDStateConfig();
        }
    }
}