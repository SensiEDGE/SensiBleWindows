using MaterialDesignThemes.Wpf;
using SensiEdge.Data;
using SensiEdge.Device;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SensiEdgeDemo.Domain
{
    public class SmokeViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetView((SmokeSensor)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
        private PackIconKind GetView(SmokeSensor value)
        {
            if (value is null) return PackIconKind.BellSleep;
            return value.Smoke == SmokeStateType.Alarm ? PackIconKind.BellRing : PackIconKind.BellOutline;
        }
    }
    public class SmokeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetText((SmokeSensor)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
        private string GetText(SmokeSensor value)
        {
            if (value is null) return "";
            return $"Smoke: {GetStatus(value.Smoke)}, Time Slot A: {value.TimeSlotA}, Time Slot B: {value.TimeSlotB}";
        }

        private string GetStatus(SmokeStateType status)
        {
            switch (status)
            {
                case SmokeStateType.Alarm:
                    return "Alarm";
                case SmokeStateType.NoAlarm:
                    return "No Alarm";
                default:
                    return "Unknown";
            }
        }
    }
    public class SmokeSensorViewModel : INotifyPropertyChanged
    {
        private ISource<SmokeSensor> Source { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private SmokeSensor smokeSensor;
        public SmokeSensor SmokeSensor
        {
            get { return smokeSensor; }
            set { this.MutateVerbose(ref smokeSensor, value, RaisePropertyChanged()); }
        }

        public SmokeSensorViewModel(ISource<SmokeSensor> source)
        {
            Source = source;
            Source.OnChange += (SmokeSensor value) => SmokeSensor = value;
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        internal void Activate()
        {
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }
    }
}
