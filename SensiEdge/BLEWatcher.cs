using System.Collections.Concurrent;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using AdvWatcher = Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisementWatcher;
using AdvStatus = Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisementWatcherStatus;
using System;
using SensiEdge.Device;
using Windows.Devices.Enumeration;

namespace SensiEdge
{
    public delegate void OnChange();
    public static class BleWatcher
    {
        private const string SENSI = "Sensi";
        public static AdvStatus Status => Watcher.Status;
        public static ConcurrentDictionary<ulong, DeviceModel> Devices { get; private set; }
        
        private static AdvWatcher Watcher { get; set; }
        
        public static event OnChange Changed = null;

        static BleWatcher()
        {
            Devices = new ConcurrentDictionary<ulong, DeviceModel>();
            Watcher = new AdvWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active,
                //AdvertisementFilter = new BluetoothLEAdvertisementFilter(){ }.Advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData() { })
            };

            Watcher.Received += (sender, args) =>
            {
                if (args.Advertisement.LocalName.Contains(SENSI))
                {
                    if (!Devices.ContainsKey(args.BluetoothAddress))
                    {
                        var device = new DeviceModel()
                        {
                            Name = args.Advertisement.LocalName,
                            BluetoothAddress = args.BluetoothAddress
                        };
                        if (Devices.TryAdd(args.BluetoothAddress, device))
                            Changed?.Invoke();
                    }
                }
            };


            //Watcher.Removed += (DeviceWatcher sender, DeviceInformationUpdate args) =>
            //{
            //    DeviceInformation info;
            //    if (Devices.TryRemove(args.Id, out info))
            //        Changed?.Invoke();
            //};
        }

        public static void StartWatch()
        {
            if (Status == AdvStatus.Created ||
               Status == AdvStatus.Aborted ||
               Status == AdvStatus.Stopped)
               Watcher.Start();
        }
        public static void StopWatch()
        {
            Watcher.Stop();
        }
        public static void ResetDevices()
        {
            Devices.Clear();
        }
    }
}
