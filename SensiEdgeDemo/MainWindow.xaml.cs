using FirstFloor.ModernUI.Windows.Controls;
using SensiEdge;
using SensiEdgeDemo.Pages;
using System.Windows;

namespace SensiEdgeDemo
{
    /// <summary>
    /// Логика взаимодействия для Mui2MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            SettingsAppearanceViewModel settingsAppearance = new SettingsAppearanceViewModel();
            InitializeComponent();
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Appearance.Default.Save();
            Properties.AzureSettings.Default.Save();
            Properties.AmazonSettings.Default.Save();
            Properties.IBMWatsonsSettings.Default.Save();
        }

        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BleWatcher.StartWatch();
        }
    }
}
