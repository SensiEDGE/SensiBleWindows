using MaterialDesignThemes.Wpf;
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
using System.Windows.Input;

namespace SensiEdgeDemo.Domain
{
    public class LedSwitchCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ISource<LEDStateConfig> Source { get; set; }
        public LedSwitchCommand(ISource<LEDStateConfig> source)
        {
            Source = source;
        }
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (Source.IsAvailable)
                Source.SetValue(new LEDStateConfig());
        }
    }
    public class LedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((LEDStateType)value == LEDStateType.On) ? PackIconKind.LedOn : PackIconKind.LedOff;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class LedStateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ISource<LEDState> Source { get; set; }
        private ISource<LEDStateConfig> ConfigSource { get; set; }
        public ICommand Switch { get; set; }
        private LEDState ledState;
        public LEDState LedState
        {
            get { return ledState; }
            set { this.MutateVerbose(ref ledState, value, RaisePropertyChanged()); }
        }

        internal async void Activate()
        {
            this.LedState = await Source.GetValue();
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }

        public LedStateViewModel(ISource<LEDState> source, ISource<LEDStateConfig> configSource)
        {
            Source = source;
            ConfigSource = configSource;
            Source.OnChange += (LEDState value) => LedState = value;
            Switch = new LedSwitchCommand(ConfigSource);
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
