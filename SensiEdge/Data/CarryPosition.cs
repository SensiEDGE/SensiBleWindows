using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public enum CarryType
    {
        Unknown = 0x00,
        OnDesc = 0x01,
        InHand = 0x02,
        NearHead = 0x03,
        ShirtPocket = 0x04,
        TrouserPocket = 0x05,
        ArmSwing = 0x06,
        JacketPocket = 0x07
    }
    public class CarryPosition : IParse
    {
        public ushort TimeStamp { get; set; }
        public CarryType Position { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Position = (CarryType)data[2];
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
