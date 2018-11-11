using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using SensiEdge.Device;

namespace SensiEdgeDemo.Domain
{
    public class PressureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Pressure: {0.01 * System.Convert.ToInt32(value)} [mBar]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class HumidityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Humidity: {0.1 * System.Convert.ToInt32(value)} [%]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class TemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Temperature: {0.1 * System.Convert.ToInt32(value)} [°C]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class EnviromentalViewModel : INotifyPropertyChanged
    {
        private ISource<Environmental> Source { get; set; }
        private Environmental environmental;
        public Environmental Environmental
        {
            get { return environmental; }
            set { this.MutateVerbose(ref environmental, value, RaisePropertyChanged()); }
        }
        public EnviromentalViewModel(ISource<Environmental> source)
        {
            Source = source;
            Source.OnChange += (Environmental value) => Environmental = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        internal void Activate()
        {
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
