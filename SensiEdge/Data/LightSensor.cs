using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public class LightSensor : IParse
    {
        public ushort TimeStamp { get; set; }
        public ushort Value { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Value = BitConverter.ToUInt16(data, 2);
        }

        public byte[] ToSetValue() => throw new NotImplementedException();
    }
}
