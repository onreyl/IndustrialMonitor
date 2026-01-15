using IndustrialMonitor.Services;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// Simple service locator / DI container.
    /// In a larger application, use a proper DI framework like:
    /// - Microsoft.Extensions.DependencyInjection
    /// - Autofac
    /// - Unity
    /// </summary>
    public class ViewModelLocator
    {
        private static ViewModelLocator? _instance;
        public static ViewModelLocator Instance => _instance ??= new ViewModelLocator();

        // Services (singletons)
        private readonly IPlcService _plcService;
        private readonly DataService _dataService;

        // ViewModels
        private MainViewModel? _mainViewModel;
        private ControlPanelViewModel? _controlPanelViewModel;
        private ProcessOverviewViewModel? _processOverviewViewModel;
        private StatusBarViewModel? _statusBarViewModel;

        public ViewModelLocator()
        {
            // Register services
            _plcService = new PlcService();
            _dataService = new DataService(_plcService);
            _dataService.Start();
        }

        // Service accessors
        public IPlcService PlcService => _plcService;
        public DataService DataService => _dataService;

        // ViewModel accessors (lazy initialization)
        public MainViewModel MainViewModel =>
            _mainViewModel ??= new MainViewModel(_dataService);

        public ControlPanelViewModel ControlPanelViewModel =>
            _controlPanelViewModel ??= new ControlPanelViewModel(_dataService);

        public ProcessOverviewViewModel ProcessOverviewViewModel =>
            _processOverviewViewModel ??= new ProcessOverviewViewModel(_dataService);

        public StatusBarViewModel StatusBarViewModel =>
            _statusBarViewModel ??= new StatusBarViewModel(_dataService);

        public void Cleanup()
        {
            _dataService.Dispose();
        }
    }
}
