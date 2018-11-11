using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public enum GestureType
    {
        NoGesture = 0x00,
        Pickup = 0x01,
        Glance = 0x02,
        Wakeup = 0x03
    }
    public class GestureRecognition : IParse
    {
        public ushort TimeStamp { get; set; }
        public GestureType Gesture { get; set; }
        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Gesture = (GestureType)data[2];
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
