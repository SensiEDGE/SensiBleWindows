using System.Windows.Controls;

namespace SensiEdgeDemo.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsAppearance.xaml
    /// </summary>
    public partial class SettingsAppearance : UserControl
    {
        public SettingsAppearance()
        {
            InitializeComponent();
            DataContext = new SettingsAppearanceViewModel();
        }
    }
}
