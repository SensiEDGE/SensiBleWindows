using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdge.Data
{
    public enum AccererometerEventTypes
    {
        Flags,
        Steps
    }
    public class AccelerometerEvents : IParse
    {
        public Type Type { get; set; }
        public ushort TimeStamp { get; set; }
        public byte EventFlags { get; set; }
        public byte CountLow { get; set; }
        public byte CountHigh { get; set; }

        public void Parse(byte[] data)
        {
            //TODO
            throw new NotImplementedException();
        }

        public byte[] ToSetValue() => throw new NotSupportedException();
    }
}
