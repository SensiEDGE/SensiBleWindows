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
    /// Логика взаимодействия для Connections.xaml
    /// </summary>
    public partial class DevicesView : UserControl
    {
        public DevicesView()
        {
            InitializeComponent();
            var model = new MainWindowViewModel();
            model.DeviceChanged += () => { DemoItemsListBox.SelectedIndex = 0; };
            DataContext = model;
            DemoItemsListBox.SelectionChanged += DemoItemsListBox_SelectionChanged;
        }

        private void DemoItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DemoItemsListBox.SelectedValue != null && DemoItemsListBox.SelectedValue is DemoItem)
            {
                //PageName.Text = ((DemoItem)DemoItemsListBox.SelectedValue).Name;
            }
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
