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
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Collections.Concurrent;
using System.Windows.Media;
using System.Windows.Threading;
using System.Timers;

namespace SensiEdgeDemo.Domain
{
    public class AccGyroMagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"x: {System.Convert.ToInt32((value as Vector3D).X)}, y: {System.Convert.ToInt32((value as Vector3D).Y)}, z: {System.Convert.ToInt32((value as Vector3D).Z)},";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class AccGyroMagViewModel : INotifyPropertyChanged, IDisposable
    {
        private ISource<AccGyroMag> Source;
        private AccGyroMag accGyroMag;
        public Func<double, string> Formatter { get; set; } = value => $"{value:F2}";
        public AccGyroMag AccGyroMag
        {
            get { return accGyroMag; }
            set { this.MutateVerbose(ref accGyroMag, value, RaisePropertyChanged()); }
        }
        private IList<KeyValuePair<double, AccGyroMag>> Values = new List<KeyValuePair<double, AccGyroMag>>();
        SeriesCollection seriesCollection = new SeriesCollection();
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { this.MutateVerbose(ref seriesCollection, value, RaisePropertyChanged()); }
        }
        private Timer Timer = new Timer(1000);
        private int TimeCicles = 0;
        private const int TIME_RANGE = 3;
        private const double TIME_INTERVAL = 0.1;
        public int SelectedIndex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AccGyroMagViewModel(ISource<AccGyroMag> source)
        {
            Source = source;
            Source.OnChange += (AccGyroMag value) => { Push(value); };
            Timer.Elapsed += (object sender, ElapsedEventArgs e) => { SetView(); };
            Timer.Start();
        }


        private void Push(AccGyroMag value)
        {
            lock (this)
            {
                var count = Values.Count;
                if (count == 0)
                {
                    Values.Add(new KeyValuePair<double, AccGyroMag>(GetTime(value.TimeStamp), value));
                }
                else
                {
                    var time = GetTime(value.TimeStamp);
                    var last = Values[count - 1];
                    if (last.Key > time + ushort.MaxValue * TimeCicles) TimeCicles++;
                    time += ushort.MaxValue * TimeCicles;
                    if (last.Key + TIME_INTERVAL > time) return;
                    Values.Add(new KeyValuePair<double, AccGyroMag>(time, value));
                    while (Values[0].Key + TIME_RANGE < time)
                        Values.RemoveAt(0);
                }
            }
        }
        private void SetView()
        {
            try
            {
                lock (this)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (Values == null) return;
                        var x = new LineSeries() { Fill = Brushes.Transparent, Foreground = Brushes.Red, PointGeometry = null, Values = new ChartValues<ObservablePoint>() };
                        var y = new LineSeries() { Fill = Brushes.Transparent, Foreground = Brushes.Green, PointGeometry = null, Values = new ChartValues<ObservablePoint>() };
                        var z = new LineSeries() { Fill = Brushes.Transparent, Foreground = Brushes.Blue, PointGeometry = null, Values = new ChartValues<ObservablePoint>() };
                        foreach (var kvp in Values)
                        {
                            switch (SelectedIndex)
                            {
                                case 0:
                                    x.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Acc.X)));
                                    y.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Acc.Y)));
                                    z.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Acc.Z)));
                                    break;
                                case 1:
                                    x.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Gyro.X)));
                                    y.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Gyro.Y)));
                                    z.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Gyro.Z)));
                                    break;
                                case 2:
                                    x.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Mag.X)));
                                    y.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Mag.Y)));
                                    z.Values.Add(new ObservablePoint(kvp.Key, GetValue(kvp.Value.Mag.Z)));
                                    break;
                            }
                        }
                        if (Values.Count > 0)
                        {
                            AxisMin = Values[0].Key;
                            AxisMax = Values[Values.Count - 1].Key;
                        }
                        SeriesCollection = new SeriesCollection() { x, y, z };
                    });
                }
            }
            catch { }
        }


        private double GetTime(ushort value) => value / 100.0;
        private double GetValue(int value) => value / 1000.0;
        private double _axisMax;
        public double AxisMax
        {
            get { return _axisMax; }
            set { this.MutateVerbose(ref _axisMax, value, RaisePropertyChanged()); }
        }
        private double _axisMin;
        public double AxisMin
        {
            get { return _axisMin; }
            set { this.MutateVerbose(ref _axisMin, value, RaisePropertyChanged()); }
        }


        //задание порогов отображения оси х


        internal void Activate()
        {
            Source.Enable();
        }

        public void Deactivate()
        {
            Source.Disable();
        }

        public Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        public void Dispose()
        {
            Timer.Stop();
        }
    }
}
