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
    public class ProximityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Proximity: {System.Convert.ToInt32(value)} [cm]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class ProximityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ISource<Proximity> Source;
        private Proximity proximity;
        public Proximity Proximity
        {
            get { return proximity; }
            set { this.MutateVerbose(ref proximity, value, RaisePropertyChanged()); }
        }

        internal void Activate()
        {
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }

        public ProximityViewModel(ISource<Proximity> source)
        {
            Source = source;
            Source.OnChange += (Proximity value) => Proximity = value;
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
