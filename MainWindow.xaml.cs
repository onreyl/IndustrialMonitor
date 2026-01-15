using System.Windows;

namespace IndustrialMonitor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // DataContext is now set via ViewModelLocator in XAML
        }
    }
}
