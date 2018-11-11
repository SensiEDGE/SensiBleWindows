using System;

namespace SensiEdge.Device
{
    public class Environmental : IParse
    {
        public ushort TimeStamp { get; set; }
        public int Pressure { get; set; }
        public short Humidity { get; set; }
        public short Temperature { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Pressure = BitConverter.ToInt32(data, 2);
            Humidity = BitConverter.ToInt16(data, 6);
            Temperature = BitConverter.ToInt16(data, 8);
        }

        public byte[] ToSetValue()
        {
            throw new NotSupportedException();
        }
    }
}
