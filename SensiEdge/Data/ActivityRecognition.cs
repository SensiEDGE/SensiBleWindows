using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public enum ActivityType
    {
        NoActivity = 0x00,
        Stationary = 0x01,
        Walking = 0x02,
        FastWalking = 0x03,
        Jogging = 0x04,
        Biking = 0x05,
        Driving = 0x06
    }
    public class ActivityRecognition : IParse
    {
        public ushort TimeStamp { get; set; }
        public ActivityType Activity { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Activity = (ActivityType)data[2];
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
