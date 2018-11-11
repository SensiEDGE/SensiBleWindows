using SensiEdgeDemo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensiEdgeDemo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProximityView.xaml
    /// </summary>
    public partial class ProximityView : UserControl
    {
        public ProximityView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProximityViewModel)
            {
                ((ProximityViewModel)DataContext).Activate();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProximityViewModel)
            {
                ((ProximityViewModel)DataContext).Deactivate();
            }
        }
    }
}
