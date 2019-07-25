using SensiEdge;
using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using AdvStatus = Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisementWatcherStatus;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using System.Text.RegularExpressions;

namespace SensiEdgeDemo.Domain
{
    public delegate void DeviceSelected(string id);
    public class SearchCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action Action = null;
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Action?.Invoke();
    }

    public class SelectCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<ulong> OnExecute = null;
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => OnExecute?.Invoke((ulong)parameter);
    }
    public class CodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string)value;
            if (string.IsNullOrEmpty(name))
                return "_";
            return name[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class BluetoothAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bluetoothAddress = (ulong)value;
            var tempMac = bluetoothAddress.ToString("X");
            var regex = "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})";
            var replace = "$1:$2:$3:$4:$5:$6";
            var macAddress = Regex.Replace(tempMac, regex, replace);
            return macAddress;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class FindDevicesViewModel : INotifyPropertyChanged
    {
        public IList<DeviceModel> devices;

        public event PropertyChangedEventHandler PropertyChanged;
        public FindDevicesViewModel(Action<ulong> action)
        {
            BleWatcher.Changed += () => Devices = BleWatcher.Devices.Values.ToList();
            ((SelectCommand)Select).OnExecute += action;
            ((SearchCommand)Search).Action += () =>
            {
                if (BleWatcher.Status != AdvStatus.Started)
                {
                    BleWatcher.StartWatch();
                }
                else
                {
                    Devices = null;
                    BleWatcher.ResetDevices();
                }
            };
            Devices = BleWatcher.Devices.Values.ToList();
        }
        public IList<DeviceModel> Devices
        {
            get { return devices; }
            set
            {
                this.MutateVerbose(ref devices, value, RaisePropertyChanged());
                if (SelectedDevice.LastConectedId != 0 && value != null)
                {
                    if (value.Any(i => i.BluetoothAddress == SelectedDevice.LastConectedId) && SelectedDevice.Connected == false)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate { Select.Execute(SelectedDevice.LastConectedId); });
                        SelectedDevice.Connected = true;
                    }
                }
            }
        }
        public ICommand Search { get; } = new SearchCommand();
        public ICommand Select { get; } = new SelectCommand();
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
