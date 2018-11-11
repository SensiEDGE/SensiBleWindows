using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdge.Data
{
    public class UltraViolet : IParse
    {
        public ushort TimeStamp { get; set; }
        public ushort Value { get; set; }
        public void Parse(byte[] data)
        {
            TimeStamp = BitConverter.ToUInt16(data, 0);
            Value = BitConverter.ToUInt16(data, 2);
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
