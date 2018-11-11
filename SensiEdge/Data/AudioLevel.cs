using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public class AudioLevel : IParse
    {
        public ushort TimeStamp { get; set; }
        public byte Level { get; set; }
        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Level = data[2];
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
