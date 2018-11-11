using SensiEdge.Device;
using System;

namespace SensiEdge.Data
{

    public class AccGyroMag : IParse
    {
        public ushort TimeStamp { get; set; }
        public Vector3D Acc { get; set; }
        public Vector3D Gyro { get; set; }
        public Vector3D Mag { get; set; }
        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Acc = new Vector3D();
            Acc.Parse(data, 2);
            Gyro = new Vector3D();
            Gyro.Parse(data, 8);
            Mag = new Vector3D();
            Mag.Parse(data, 14);
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
