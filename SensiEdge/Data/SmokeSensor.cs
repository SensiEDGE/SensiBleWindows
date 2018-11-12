using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdge.Data
{
    public enum SmokeStateType
    {
        NoAlarm = 0x00,
        Alarm = 0x01
    }
    public class SmokeSensor : IParse
    {
        public ushort TimeStamp { get; set; }
        public ushort TimeSlotA { get; set; }
        public ushort TimeSlotB { get; set; }
        public SmokeStateType Smoke { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            TimeSlotA = BitConverter.ToUInt16(data, 2);
            TimeSlotB = BitConverter.ToUInt16(data, 4);
            Smoke = (SmokeStateType)data[6];
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}