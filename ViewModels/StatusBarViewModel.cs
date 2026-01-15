using System.Windows.Input;
using IndustrialMonitor.Core;
using IndustrialMonitor.Services;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// ViewModel for the Status Bar / Footer view.
    /// Handles PLC connection status and controls.
    /// </summary>
    public class StatusBarViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        #region Properties

        private bool _isSimulationMode = true;
        public bool IsSimulationMode
        {
            get => _isSimulationMode;
            set
            {
                if (SetProperty(ref _isSimulationMode, value))
                {
                    ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)DisconnectCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private string _connectionStatus = "Disconnected - Simulation Mode";
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }

        public string ModeText => IsSimulationMode ? "SIMULATION" : "PLC";

        #endregion

        #region Commands

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        #endregion

        public StatusBarViewModel(DataService dataService)
        {
            _dataService = dataService;

            ConnectCommand = new RelayCommand(OnConnect, _ => IsSimulationMode);
            DisconnectCommand = new RelayCommand(OnDisconnect, _ => !IsSimulationMode);

            // Subscribe to status changes
            _dataService.StatusChanged += OnStatusChanged;

            // Initialize
            IsSimulationMode = _dataService.IsSimulationMode;
        }

        private void OnConnect(object? parameter)
        {
            ConnectionStatus = "Connecting...";
            bool connected = _dataService.ConnectPlc();
            IsSimulationMode = _dataService.IsSimulationMode;

            // Notify other ViewModels
            Messenger.Default.Send(new PlcConnectionChangedMessage(
                connected,
                IsSimulationMode,
                ConnectionStatus));
        }

        private void OnDisconnect(object? parameter)
        {
            _dataService.DisconnectPlc();
            IsSimulationMode = true;
            ConnectionStatus = "Disconnected - Simulation Mode";

            // Notify other ViewModels
            Messenger.Default.Send(new PlcConnectionChangedMessage(
                false,
                true,
                ConnectionStatus));
        }

        private void OnStatusChanged(object? sender, string status)
        {
            ConnectionStatus = status;
            IsSimulationMode = _dataService.IsSimulationMode;
            OnPropertyChanged(nameof(ModeText));
        }
    }
}
