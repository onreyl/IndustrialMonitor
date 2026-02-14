using Microsoft.Extensions.DependencyInjection;
using S7.Net;
using IndustrialMonitor.Services;
using System;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// Responsible for locating and providing ViewModel instances across the application.
    /// Initializes the dependency injection container and registers shared services and ViewModels.
    /// </summary>
    public class ViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelLocator()
        {
            var services = new ServiceCollection();

            // Register PlcService
            // In a production environment, configuration should be loaded from appsettings.json
            services.AddSingleton<PlcService>(sp => new PlcService(CpuType.S71200, "127.0.0.1", 0, 1, true));

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ControlPanelViewModel>();
            services.AddSingleton<StatusBarViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel Main => _serviceProvider.GetRequiredService<MainViewModel>();
        public ControlPanelViewModel ControlPanelViewModel => _serviceProvider.GetRequiredService<ControlPanelViewModel>();
        public StatusBarViewModel StatusBarViewModel => _serviceProvider.GetRequiredService<StatusBarViewModel>();

        /// <summary>
        /// Cleans up resources by disposing the service provider if applicable.
        /// </summary>
        public void Cleanup()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
