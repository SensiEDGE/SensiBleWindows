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
    public class AudioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Audio Level: {System.Convert.ToInt32(value)} [dB]";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class AudioLevelViewModel : INotifyPropertyChanged
    {
        private ISource<AudioLevel> Source;
        private AudioLevel audioLevel;
        public AudioLevel AudioLevel
        {
            get { return audioLevel; }
            set { this.MutateVerbose(ref audioLevel, value, RaisePropertyChanged()); }
        }

        internal void Activate()
        {
            Source.Enable();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void Deactivate()
        {
            Source.Disable();
        }

        public AudioLevelViewModel(ISource<AudioLevel> source)
        {
            Source = source;
            Source.OnChange += (AudioLevel value) => AudioLevel = value;
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
