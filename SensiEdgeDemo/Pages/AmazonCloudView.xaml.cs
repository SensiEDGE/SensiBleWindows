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
    /// Логика взаимодействия для AmazonCloudView.xaml
    /// </summary>
    public partial class AmazonCloudView : UserControl
    {
        public AmazonCloudView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AmazonCloudViewModel)
            {
                ((AmazonCloudViewModel)DataContext).Activate();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AmazonCloudViewModel)
            {
                ((AmazonCloudViewModel)DataContext).Deactivate();
            }
            Properties.AmazonSettings.Default.Save();
        }

        private void RootCADownload_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".pem",
                Filter = "Certificates (*.pem)|*.pem"
            };

            bool? result = dlg.ShowDialog();
            
            if (result == true)
            {
                string filename = dlg.FileName;
                RootCertificateTextBox.Text = filename;
                Properties.AmazonSettings.Default.RootCA = filename;
            }
        }

        private void DeviceCADownload_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".pfx";
            dlg.Filter = "Certificates (*.pfx)|*.pfx";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                DeviceCertificateTextBox.Text = filename;
                Properties.AmazonSettings.Default.DeviceCA = filename;
            }
        }       
    }
}
