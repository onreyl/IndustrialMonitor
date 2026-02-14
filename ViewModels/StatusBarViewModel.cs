using IndustrialMonitor.Services;
using System.Windows.Input;
using System.Threading.Tasks;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// ViewModel for the status bar, handling connection status display and global simulation settings.
    /// </summary>
    public class StatusBarViewModel : BaseViewModel
    {
        private readonly PlcService _plcService;
        private string _connectionStatus = "DISCONNECTED";
        private bool _isSimulationMode = true;

        public StatusBarViewModel(PlcService plcService)
        {
            _plcService = plcService;
            ConnectCommand = new RelayCommand(async _ => await Connect());
            DisconnectCommand = new RelayCommand(_ => Disconnect());
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }

        public bool IsSimulationMode
        {
            get => _isSimulationMode;
            set => SetProperty(ref _isSimulationMode, value);
        }

        private async Task Connect()
        {
            ConnectionStatus = "CONNECTING...";
            await _plcService.ConnectAsync();
            ConnectionStatus = "CONNECTED";
        }

        private void Disconnect()
        {
            _plcService.Disconnect();
            ConnectionStatus = "DISCONNECTED";
        }
    }
}
