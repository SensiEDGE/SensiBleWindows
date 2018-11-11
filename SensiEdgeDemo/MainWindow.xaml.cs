using SensiEdge;
using SensiEdge.Device;
using SensiEdge.Data;
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
using SensiEdgeDemo.Domain;
using System.Diagnostics;
using MaterialDesignThemes.Wpf;
using Windows.UI.Xaml.Controls.Primitives;
using System.Threading;

namespace SensiEdgeDemo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var model = new MainWindowViewModel();
            model.DeviceChanged += () => { DemoItemsListBox.SelectedIndex = 0; };
            DataContext = model;
            DemoItemsListBox.SelectionChanged += DemoItemsListBox_SelectionChanged;
        }

        private void DemoItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DemoItemsListBox.SelectedValue != null && DemoItemsListBox.SelectedValue is DemoItem)
            {
                PageName.Text = ((DemoItem)DemoItemsListBox.SelectedValue).Name;
            }
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }
    }
}
