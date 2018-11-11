using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public enum LEDStateType
    {
        Off = 0x00,
        On = 0x01
    }
    public class LEDState : IParse
    {
        public ushort TimeStamp { get; set; }
        public LEDStateType State { get; set; }
        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            State = (LEDStateType)data[2];
        }

        public byte[] ToSetValue() => new byte[] { (byte)State};

    }
}
