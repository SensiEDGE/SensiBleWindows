using SensiEdgeDemo.Pages;
using System.Collections.Generic;
using SensiEdge.Device;
using System;
using SensiEdge;
using System.ComponentModel;
using SensiEdge.Data;
using System.Windows.Input;
using SensiEdgeDemo.Demo;

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
            FindMode = new DemoCommand() { Executer = () => SetDevice(null) };
            SetDevice(null);
        }

        private async void SetDeviceFromIdAsync(string Id)
        {
            try
            {
                Device = await DeviceFactory.Get(Id);
                SetDevice(Device);
            }
            catch { }
        }

        private void SetDevice(IDevice device)
        {
            if (device is null)
            {
                DemoItems = new List<DemoItem>()
                {
                    new DemoItem("Find Devices", new FindDevicesView(){
                        DataContext = new FindDevicesViewModel(new Action<string>((string id) => {
                            SetDeviceFromIdAsync(id);
                        }))
                    })
                };
            }
            else
            {
                DemoItems = new List<DemoItem>();
                if (device.EnvironmentalSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Enviroment", new EnvironmentalView()
                    {
                        DataContext = new EnviromentalViewModel(device.EnvironmentalSource)
                    }));
                if (device.AccGyroMagSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Acc, Gyro and Mag", new AccGyroMagView()
                    {
                        DataContext = new AccGyroMagViewModel(device.AccGyroMagSource)
                    }));
                if (device.AudioLevelSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Audio Level", new AudioLevelView()
                    {
                        DataContext = new AudioLevelViewModel(device.AudioLevelSource)
                    }));
                if (device.LEDStateSource.IsAvailable)
                    DemoItems.Add(new DemoItem("LED State", new LedStateView()
                    {
                        DataContext = new LedStateViewModel(device.LEDStateSource, device.LEDStateConfigSource)
                    }));
                if (device.LightSensorSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Light Sensor", new LightSensorView()
                    {
                        DataContext = new LightSensorViewModel(device.LightSensorSource)
                    }));
                if (device.BatteryStatusSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Battery Status", new BatteryStatusView()
                    {
                        DataContext = new BatteryStatusViewModel(device.BatteryStatusSource)
                    }));
                if (device.OrientationSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Orientation", new OrientationView()
                    {
                        DataContext = new OrientationViewModel(device.OrientationSource)
                    }));
                if (device.CompassSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Compass", new CompassView()
                    {
                        DataContext = new CompassViewModel(device.CompassSource)
                    }));
                if (device.ProximitySource.IsAvailable)
                    DemoItems.Add(new DemoItem("Proximity", new ProximityView()
                    {
                        DataContext = new ProximityViewModel(device.ProximitySource)
                    }));
                if (device.UltraVioletSource.IsAvailable)
                    DemoItems.Add(new DemoItem("Ultra Violet", new UltraVioletView()
                    {
                        DataContext = new UltraVioletViewModel(device.UltraVioletSource)
                    }));
                DemoItems.Add(new DemoItem("Cloud", new AzureCloudView()
                {
                    DataContext = new AzureCloudViewModel(device)
                }));
            }
            DeviceChanged?.Invoke();
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}