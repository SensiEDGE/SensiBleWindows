using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdge.Data
{
    public class Compass : IParse
    {
        public ushort TimeStamp { get; set; }
        public ushort Angle { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Angle = BitConverter.ToUInt16(data, 2);
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
