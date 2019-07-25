using SensiEdgeDemo.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SensiEdgeDemo.Pages
{
    /// <summary>
    /// Логика взаимодействия для Compass.xaml
    /// </summary>
    public partial class CompassView : UserControl
    {
        public CompassView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CompassViewModel)
            {
                ((CompassViewModel)DataContext).Activate();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CompassViewModel)
            {
                ((CompassViewModel)DataContext).Deactivate();
            }
        }
    }
}
