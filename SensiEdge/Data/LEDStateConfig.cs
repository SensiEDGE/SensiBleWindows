using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdge.Data
{
    public class LEDStateConfig : IParse
    {
        private byte[] valueInvertor = new byte[] { 0x20, 0x00, 0x00, 0x00 };
        public void Parse(byte[] data) => throw new NotSupportedException();

        public byte[] ToSetValue() => valueInvertor;
    }
}
