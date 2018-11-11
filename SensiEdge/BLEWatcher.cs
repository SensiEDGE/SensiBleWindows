using System.Collections.Concurrent;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace SensiEdge
{
    public delegate void OnChange();
    public static class BleWatcher
    {
        public static DeviceWatcherStatus Status => Watcher.Status;
        public static ConcurrentDictionary<string, DeviceInformation> Devices { get; private set; }
        private static DeviceWatcher Watcher { get; set; }
        public static event OnChange Changed = null;

        static BleWatcher()
        {
            Devices = new ConcurrentDictionary<string, DeviceInformation>();
            Watcher = DeviceInformation.CreateWatcher(
                $"{BluetoothLEDevice.GetDeviceSelectorFromPairingState(true)} OR " +
                $"{BluetoothLEDevice.GetDeviceSelectorFromPairingState(false)}");
            Watcher.Added += (DeviceWatcher sender, DeviceInformation args) =>
            {
                if (Devices.TryAdd(args.Id, args))
                    Changed?.Invoke();
            };
            Watcher.Removed += (DeviceWatcher sender, DeviceInformationUpdate args) =>
            {
                DeviceInformation info;
                if (Devices.TryRemove(args.Id, out info))
                    Changed?.Invoke();
            };
        }

        public static void StartWatch()
        {
            if(Status == DeviceWatcherStatus.Created ||
               Status == DeviceWatcherStatus.Aborted ||
               Status == DeviceWatcherStatus.Stopped)
            Watcher.Start();
        }
        public static void StopWatch()
        {
            Watcher.Stop();
        }
    }
}
