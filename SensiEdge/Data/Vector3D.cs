using System;

namespace SensiEdge.Data
{
    public class Vector3D
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public void Parse(byte[] data, int index)
        {
            X = BitConverter.ToInt16(data, index);
            Y = BitConverter.ToInt16(data, index + 2);
            Z = BitConverter.ToInt16(data, index + 4);
        }
    }
}
