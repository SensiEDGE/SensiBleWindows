using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public enum BatteryStatusType
    {
        Discharging = 0x01,
        Charging = 0x02,
        Unknown = 0x04
    }
    public class BatterySatus : IParse
    {
        public ushort TimeStamp { get; set; }
        public ushort SOC { get; set; }
        public ushort Voltage { get; set; }
        public ushort Current { get; set; }
        public BatteryStatusType Status { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            SOC = BitConverter.ToUInt16(data, 2);
            Voltage = BitConverter.ToUInt16(data, 4);
            Current = BitConverter.ToUInt16(data, 6);
            Status = (BatteryStatusType)data[8];
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
