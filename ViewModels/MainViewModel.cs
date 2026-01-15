using IndustrialMonitor.Core;
using IndustrialMonitor.Services;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// Main ViewModel - coordinates application-level concerns.
    /// In this simple app, it mainly serves as the root DataContext.
    /// In larger apps, it would handle navigation, dialogs, etc.
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        // In a more complex app, you might have:
        // - Navigation commands
        // - Application-wide settings
        // - Dialog management
        // - Logging coordination
    }
}
