using SensiEdgeDemo.Pages;
using System.Collections.Generic;
using SensiEdge.Device;
using System;
using SensiEdge;
using System.ComponentModel;
using SensiEdge.Data;
using System.Windows.Input;
using SensiEdgeDemo.Demo;
using Windows.Devices.Bluetooth;
using System.Threading;

namespace SensiEdgeDemo.Domain
{
    public delegate void DeviceChanged();
    public class DemoCommand : ICommand
    {
        public Action Executer { get; set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Executer?.Invoke();
    }
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public DevicesView Window { get; set; }
        private IDevice Device { get; set; }
        private IList<DemoItem> demoItems;
        public event DeviceChanged DeviceChanged = null;
        public ICommand DemoMode { get; set; }
        public ICommand FindMode { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public IList<DemoItem> DemoItems
        {
            get { return demoItems; }
            set { this.MutateVerbose(ref demoItems, value, RaisePropertyChanged()); }
        }
        public MainWindowViewModel()
        {
            DemoMode = new DemoCommand() { Executer = () => SetDevice(new DemoSensiEdge()) };
            FindMode = new DemoCommand() { Executer = () => { SetDevice(null); SelectedDevice.SetBluetoothAddress(ulong.MaxValue); } };
            SetDevice(null);
        }

        private async void SetDeviceByAdddressAsync(ulong bluetoothAddress)
        {
            try
            {
                Device?.Disconnect();
                if (bluetoothAddress != ulong.MaxValue)
                {
                    Device = new SensiDevice(bluetoothAddress);
                    await Device.Connect();
                    Device.OnDisconnect += DisconectDevice;
                    SetDevice(Device);
                }
            }
            catch (Exception ex) { }
        }

        private void DisconectDevice()
        {
            //TODO: check status
            App.Current.Dispatcher.Invoke(() => { SetDevice(null); });
        }

        private void SetDevice(IDevice device)
        {
            var demoItems = new List<DemoItem>();
            if (device is null)
            {
                Device?.Disconnect();
                demoItems.Add(new DemoItem("Find Devices", new FindDevicesView()
                {
                    DataContext = new FindDevicesViewModel(new Action<ulong>((ulong bluetoothAddress) =>
                    {
                        SetDeviceByAdddressAsync(bluetoothAddress);
                    }))
                }));
            }
            else
            {
                if (device.EnvironmentalSource.IsAvailable)
                    demoItems.Add(new DemoItem("Enviroment", new EnvironmentalView()
                    {
                        DataContext = new EnviromentalViewModel(device.EnvironmentalSource)
                    }));
                if (device.AccGyroMagSource.IsAvailable)
                    demoItems.Add(new DemoItem("Acc, Gyro and Mag", new AccGyroMagView()
                    {
                        DataContext = new AccGyroMagViewModel(device.AccGyroMagSource)
                    }));
                if (device.AudioLevelSource.IsAvailable)
                    demoItems.Add(new DemoItem("Audio Level", new AudioLevelView()
                    {
                        DataContext = new AudioLevelViewModel(device.AudioLevelSource)
                    }));
                if (device.LEDStateSource.IsAvailable)
                    demoItems.Add(new DemoItem("LED State", new LedStateView()
                    {
                        DataContext = new LedStateViewModel(device.LEDStateSource, device.LEDStateConfigSource)
                    }));
                if (device.LightSensorSource.IsAvailable)
                    demoItems.Add(new DemoItem("Light Sensor", new LightSensorView()
                    {
                        DataContext = new LightSensorViewModel(device.LightSensorSource)
                    }));
                if (device.BatteryStatusSource.IsAvailable)
                    demoItems.Add(new DemoItem("Battery Status", new BatteryStatusView()
                    {
                        DataContext = new BatteryStatusViewModel(device.BatteryStatusSource)
                    }));
                if (device.OrientationSource.IsAvailable)
                    demoItems.Add(new DemoItem("Orientation", new OrientationView()
                    {
                        DataContext = new OrientationViewModel(device.OrientationSource)
                    }));
                if (device.CompassSource.IsAvailable)
                    demoItems.Add(new DemoItem("Compass", new CompassView()
                    {
                        DataContext = new CompassViewModel(device.CompassSource)
                    }));
                if (device.ProximitySource.IsAvailable)
                    demoItems.Add(new DemoItem("Proximity", new ProximityView()
                    {
                        DataContext = new ProximityViewModel(device.ProximitySource)
                    }));
                if (device.UltraVioletSource.IsAvailable)
                    demoItems.Add(new DemoItem("Ultra Violet", new UltraVioletView()
                    {
                        DataContext = new UltraVioletViewModel(device.UltraVioletSource)
                    }));
                if (device.SmokeSensorSource.IsAvailable)
                    demoItems.Add(new DemoItem("Smoke Sensor", new SmokeSensorView()
                    {
                        DataContext = new SmokeSensorViewModel(device.SmokeSensorSource)
                    }));
                demoItems.Add(new DemoItem("Azure cloud", new AzureCloudView()
                {
                    DataContext = new AzureCloudViewModel(device)
                }));
                demoItems.Add(new DemoItem("Amazon cloud", new AmazonCloudView()
                {
                    DataContext = new AmazonCloudViewModel(device)
                }));
                demoItems.Add(new DemoItem("IBMWatsons cloud", new IBMWatsonCloudView()
                {
                    DataContext = new IBMWatsonsCloudViewModel(device)
                }));
            }
            DemoItems = demoItems;
            DeviceChanged?.Invoke();
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}