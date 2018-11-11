using SensiEdge.Data;
using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SensiEdgeDemo.Domain
{
    public class LightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Light: {System.Convert.ToInt32(value)} [Lux]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class LightSensorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ISource<LightSensor> Source;
        private LightSensor lightSensor;
        public LightSensor LightSensor
        {
            get { return lightSensor; }
            set { this.MutateVerbose(ref lightSensor, value, RaisePropertyChanged()); }
        }

        internal void Activate()
        {
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }

        public LightSensorViewModel(ISource<LightSensor> source)
        {
            Source = source;
            Source.OnChange += (LightSensor value) => LightSensor = value;
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
