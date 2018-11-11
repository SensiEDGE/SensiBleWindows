using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using SensiEdge.Data;
using SensiEdge.Device;
using System.Windows.Controls;
using LiveCharts.Configurations;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace SensiEdgeDemo.Domain
{
    public class DegreeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToInt32(value) / 100;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class CompassConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Angle: {System.Convert.ToInt32(value) / 100} °";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class CompassViewModel : INotifyPropertyChanged
    {
        private ISource<Compass> Source { get; set; }
        private Compass compass;

        public Compass Compass
        {
            get { return compass; }
            set { this.MutateVerbose(ref compass, value, RaisePropertyChanged()); }
        }

        public CompassViewModel(ISource<Compass> source)
        {
            this.Source = source;
            Source.OnChange += (Compass value) =>
            {
                Compass = value;
            };
        }

        internal void Activate()
        {
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
