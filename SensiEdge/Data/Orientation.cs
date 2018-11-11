using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{
    public class Orientation : IParse
    {
        public ushort TimeStamp { get; set; }
        public Vector3D Quat0 { get; set; }
        public Vector3D Quat1 { get; set; }
        public Vector3D Quat2 { get; set; }

        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Quat0 = new Vector3D();
            Quat0.Parse(data, 2);
            Quat1 = new Vector3D();
            Quat1.Parse(data, 8);
            Quat2 = new Vector3D();
            Quat2.Parse(data, 14);
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
