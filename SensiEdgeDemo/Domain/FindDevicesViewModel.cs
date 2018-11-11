using SensiEdge;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;

namespace SensiEdgeDemo.Domain
{
    public delegate void DeviceSelected(string id);
    public class SearchCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => BleWatcher.StartWatch();
    }
    public class SelectCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<string> OnExecute = null;
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => OnExecute?.Invoke((string)parameter);
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
    public class IdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (string)value;
            if (string.IsNullOrEmpty(id) || id.Length <  35)
                return id;
            return id.Substring(id.Length - 35);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class FindDevicesViewModel : INotifyPropertyChanged
    {
        public IList<DeviceInformation> devices;

        public event PropertyChangedEventHandler PropertyChanged;
        public FindDevicesViewModel(Action<string> action)
        {
            BleWatcher.Changed += () => Devices = BleWatcher.Devices.Values.ToList();
            ((SelectCommand)Select).OnExecute += action;
            Devices = BleWatcher.Devices.Values.ToList();
        }
        public IList<DeviceInformation> Devices
        {
            get { return devices; }
            set { this.MutateVerbose(ref devices, value, RaisePropertyChanged()); }
        }
        public ICommand Search { get; } = new SearchCommand();
        public ICommand Select { get; } = new SelectCommand();
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
