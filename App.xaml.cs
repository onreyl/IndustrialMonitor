using System.Windows;
using IndustrialMonitor.ViewModels;

namespace IndustrialMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Resources["Locator"] is ViewModelLocator locator)
            {
                locator.Cleanup();
            }
        }
    }
}
