using System.Windows;
using IndustrialMonitor.ViewModels;

namespace IndustrialMonitor
{
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Cleanup resources on application exit
            if (Resources["Locator"] is ViewModelLocator locator)
            {
                locator.Cleanup();
            }
        }
    }
}
