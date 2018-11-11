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
    public class UltraVioletConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Ultra Violet: {System.Convert.ToInt32(value)} [mW/cm²]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class UltraVioletViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ISource<UltraViolet> Source;
        private UltraViolet ultraViolet;
        public UltraViolet UltraViolet
        {
            get { return ultraViolet; }
            set { this.MutateVerbose(ref ultraViolet, value, RaisePropertyChanged()); }
        }

        internal void Activate()
        {
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }

        public UltraVioletViewModel(ISource<UltraViolet> source)
        {
            Source = source;
            Source.OnChange += (UltraViolet value) => UltraViolet = value;
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
