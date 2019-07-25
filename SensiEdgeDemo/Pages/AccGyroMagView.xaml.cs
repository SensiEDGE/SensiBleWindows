using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SensiEdgeDemo.Domain;
using System.Windows;
using System.Windows.Controls;

namespace SensiEdgeDemo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccGyroMag.xaml
    /// </summary>
    public partial class AccGyroMagView : UserControl
    {
        public AccGyroMagView()
        {
            InitializeComponent();
        }
        public SeriesCollection SeriesCollection { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AccGyroMagViewModel)
            {
                ((AccGyroMagViewModel)DataContext).Activate();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AccGyroMagViewModel)
            {
                ((AccGyroMagViewModel)DataContext).Deactivate();
            }
        }
    }
}
