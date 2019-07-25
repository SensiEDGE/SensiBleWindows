using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdge.Device
{
    public static class SelectedDevice
    {
        public static ulong LastConectedId { get; private set; }
        public static bool Connected { get; set; } = false;
        static SelectedDevice()
        {
            LastConectedId = Properties.ConectedDevice.Default.BluetoothAddress;
        }
        public static void SetBluetoothAddress(ulong bluetoothAddress)
        {
            Properties.ConectedDevice.Default.BluetoothAddress = bluetoothAddress;
            LastConectedId = bluetoothAddress;
            Properties.ConectedDevice.Default.Save();
        }
    }
}
