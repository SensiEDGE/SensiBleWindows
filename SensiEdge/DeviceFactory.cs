using SensiEdge.Device;
using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace SensiEdge
{
    public delegate void DeviceAdded(DeviceInformation deviceInformation);
    public class DeviceFactory
    {
        public async static Task<IDevice> Get(string deviceId)
        {
            BluetoothLEDevice ble = await BluetoothLEDevice.FromIdAsync(deviceId);
            //TODO: check device model
            IModel model = new ModelB();
            return await SensiDevice.Get(ble, model);
        }
    }
}
