using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using SensiEdge.Data;
using SensiEdge.Device;

namespace SensiEdgeDemo.Domain
{
    public class BatteryViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetView((BatterySatus)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
        private PackIconKind GetView(BatterySatus value)
        {
            if (value is null) return PackIconKind.BatteryUnknown;
            var soc = (int)(value.SOC / 10);
            if (soc < 10) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging20 : PackIconKind.Battery10;
            else if (soc < 20) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging20 : PackIconKind.Battery20;
            else if (soc < 30) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging30 : PackIconKind.Battery30;
            else if (soc < 40) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging40 : PackIconKind.Battery40;
            else if (soc < 50) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging60 : PackIconKind.Battery50;
            else if (soc < 60) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging60 : PackIconKind.Battery60;
            else if (soc < 70) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging80 : PackIconKind.Battery70;
            else if (soc < 80) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging80 : PackIconKind.Battery80;
            else if (soc < 90) return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging90 : PackIconKind.Battery90;
            else return value.Status == BatteryStatusType.Charging ? PackIconKind.BatteryCharging100 : PackIconKind.Battery;
        }
    }
    public class BatteryStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetText((BatterySatus)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
        private string GetText(BatterySatus value)
        {
            if (value is null) return "";
            return $"Battery {GetStatus(value.Status)}, {0.1 * value.SOC} [%]";
        }

        private string GetStatus(BatteryStatusType status)
        {
            switch (status)
            {
                case BatteryStatusType.Charging:
                    return "Charging";
                case BatteryStatusType.Discharging:
                    return "Discharging";
                default:
                    return "Unknown";
            }
        }
    }
    public class VoltageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Voltage: {0.001 * System.Convert.ToInt32(value)} [V]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class CurrentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Current: {0.001 * System.Convert.ToInt32(value)} [A]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class BatteryStatusViewModel : INotifyPropertyChanged
    {
        private ISource<BatterySatus> Source { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private BatterySatus batteryStatus;
        public BatterySatus BatterySatus
        {
            get { return batteryStatus; }
            set { this.MutateVerbose(ref batteryStatus, value, RaisePropertyChanged()); }
        }

        public BatteryStatusViewModel(ISource<BatterySatus> source)
        {
            Source = source;
            Source.OnChange += (BatterySatus value) => BatterySatus = value;
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        internal async void Activate()
        {
            Source.Enable();
            this.BatterySatus = await Source.GetValue();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }
    }
}