using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using SensiEdge.Data;
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
        private Visibility pressureVisibility;
        public Visibility PressureVisibility
        {
            get { return pressureVisibility; }
            set
            {
                this.MutateVerbose(ref pressureVisibility, value, RaisePropertyChanged());
            }
        }
        private Visibility humidityVisibility;
        public Visibility HumidityVisibility
        {
            get { return humidityVisibility; }
            set
            {
                this.MutateVerbose(ref humidityVisibility, value, RaisePropertyChanged());
            }
        }
        private Visibility temperatureVisibility;
        public Visibility TemperatureVisibility
        {
            get { return temperatureVisibility; }
            set
            {
                this.MutateVerbose(ref temperatureVisibility, value, RaisePropertyChanged());
            }
        }
        private Environmental environmental;
        public Environmental Environmental
        {
            get { return environmental; }
            set
            {
                this.MutateVerbose(ref environmental, value, RaisePropertyChanged());
                if(value.Pressure==0)
                {
                    if(pressureVisibility!=Visibility.Collapsed)
                    {
                        PressureVisibility = Visibility.Collapsed;
                    }                    
                }
                else
                {
                    PressureVisibility = Visibility.Visible;
                }

                if (value.Humidity == 0)
                {
                    if (humidityVisibility != Visibility.Collapsed)
                    {
                        HumidityVisibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    HumidityVisibility = Visibility.Visible;
                }

                if (value.Temperature == 0)
                {
                    if (temperatureVisibility != Visibility.Collapsed)
                    {
                        TemperatureVisibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    TemperatureVisibility = Visibility.Visible;
                }
            }
        }
        public EnviromentalViewModel(ISource<Environmental> source)
        {
            Source = source;
            PressureVisibility = Visibility.Collapsed;
            HumidityVisibility = Visibility.Collapsed;
            TemperatureVisibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        internal void Activate()
        {
            Source.Enable();
            Source.OnChange += OnChange;
        }

        internal void Deactivate()
        {
            Source.Disable();
            Source.OnChange -= OnChange;
        }

        private void OnChange (Environmental value) => Environmental = value;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
