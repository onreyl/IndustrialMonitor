using IndustrialMonitor.Models;
using IndustrialMonitor.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// The main view model for the application, handling the primary machine status and connection logic.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly IPlcService _plcService;
        private MachineStatus _status = new MachineStatus();
        private readonly System.Timers.Timer _timer;
        private string _connectionStatus = "Disconnected";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="plcService">The PLC service for communication.</param>
        public MainViewModel(IPlcService plcService)
        {
            _plcService = plcService;

            ConnectCommand = new RelayCommand(async _ => await Connect());
            DisconnectCommand = new RelayCommand(_ => Disconnect());

            // Set up polling timer (1 second interval)
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += async (s, e) => await UpdateStatus();
        }

        /// <summary>
        /// Gets or sets the current machine status.
        /// </summary>
        public MachineStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        /// <summary>
        /// Gets or sets the connection status message.
        /// </summary>
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        private async Task Connect()
        {
            try
            {
                ConnectionStatus = "Connecting...";
                await _plcService.ConnectAsync();
                ConnectionStatus = "Connected";
                _timer.Start();
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Error: {ex.Message}";
            }
        }

        private void Disconnect()
        {
            _timer.Stop();
            _plcService.Disconnect();
            ConnectionStatus = "Disconnected";
        }

        private async Task UpdateStatus()
        {
            try
            {
                var newStatus = await _plcService.ReadStatusAsync();
                Status = newStatus;

                var app = System.Windows.Application.Current;
                if (app?.Resources["Locator"] is ViewModelLocator locator)
                {
                    locator.ControlPanelViewModel.UpdateFromStatus(newStatus);
                }
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Update Error: {ex.Message}";
            }
        }
    }
}
