using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensiEdge;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;

namespace SensiEdgeTest
{
    [TestClass]
    public class EnumerateTest
    {
        [TestMethod]
        public void TestEnumerate()
        {
            BleWatcher.Changed += () =>
            {
                foreach (var device in BleWatcher.Devices)
                {
                    Debug.WriteLine(device.Value.Name);
                }
            };
            BleWatcher.StartWatch();
            Thread.Sleep(10000);
            BleWatcher.StopWatch();
        }

        private void Factory_OnDeviceAdded(DeviceInformation deviceInformation)
        {
            Debug.WriteLine(deviceInformation.Name);
            //var connection = new BLEConnection(deviceInformation.Id);
            var Device = BluetoothLEDevice.FromIdAsync(deviceInformation.Id).GetAwaiter().GetResult();
            var items = Device.GetGattServicesAsync().GetResults().Services;
            //var item = items[0].OpenAsync(Windows.Devices.Bluetooth.GenericAttributeProfile.GattSharingMode);
        }

        [TestMethod]
        public void TestFindAll()
        {
            var devices =  DeviceInformation.FindAllAsync(
               $"{BluetoothLEDevice.GetDeviceSelectorFromPairingState(true)} OR " +
               $"{BluetoothLEDevice.GetDeviceSelectorFromPairingState(false)}").AsTask().GetAwaiter().GetResult();

            foreach (var device in devices)
                Debug.WriteLine(device.Name);
        }

        [TestMethod]
        public void TestAdvertisement()
        {
            BluetoothLEAdvertisementWatcher watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };
            watcher.Received += OnAdvertisementReceived;
            watcher.Start();
            Thread.Sleep(1000000);
        }

        private async void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var _args = args;
            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(_args.BluetoothAddress);
            var data = args.Advertisement.ManufacturerData;
        }
    }
}
