using System.Windows.Input;
using System.Windows.Media;
using IndustrialMonitor.Core;
using IndustrialMonitor.Models;
using IndustrialMonitor.Services;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// ViewModel for the Control Panel view.
    /// Handles operator controls and displays process values.
    /// </summary>
    public class ControlPanelViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        #region Properties

        private bool _isSystemRunning;
        public bool IsSystemRunning
        {
            get => _isSystemRunning;
            set
            {
                if (SetProperty(ref _isSystemRunning, value))
                {
                    OnPropertyChanged(nameof(SystemStatusText));
                    OnPropertyChanged(nameof(SystemStatusColor));
                    ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)StopCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string SystemStatusText => IsSystemRunning ? "RUNNING" : "STOPPED";
        public Brush SystemStatusColor => IsSystemRunning ? Brushes.LimeGreen : Brushes.Red;

        private double _tankLevel;
        public double TankLevel
        {
            get => _tankLevel;
            set => SetProperty(ref _tankLevel, value);
        }

        private double _temperature;
        public double Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        private double _pressure;
        public double Pressure
        {
            get => _pressure;
            set => SetProperty(ref _pressure, value);
        }

        private int _motorSpeed;
        public int MotorSpeed
        {
            get => _motorSpeed;
            set => SetProperty(ref _motorSpeed, value);
        }

        #endregion

        #region Commands

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        #endregion

        public ControlPanelViewModel(DataService dataService)
        {
            _dataService = dataService;

            StartCommand = new RelayCommand(OnStart, _ => !IsSystemRunning);
            StopCommand = new RelayCommand(OnStop, _ => IsSystemRunning);

            // Subscribe to data updates
            _dataService.DataUpdated += OnDataUpdated;

            // Initialize with current data
            UpdateFromProcessData(_dataService.CurrentData);
        }

        private void OnStart(object? parameter)
        {
            IsSystemRunning = true;
            _dataService.SetSystemRunning(true);

            // Notify other ViewModels
            Messenger.Default.Send(new SystemStateChangedMessage(true));
        }

        private void OnStop(object? parameter)
        {
            IsSystemRunning = false;
            _dataService.SetSystemRunning(false);

            // Notify other ViewModels
            Messenger.Default.Send(new SystemStateChangedMessage(false));
        }

        private void OnDataUpdated(object? sender, ProcessData data)
        {
            UpdateFromProcessData(data);
        }

        private void UpdateFromProcessData(ProcessData data)
        {
            TankLevel = data.TankLevel;
            Temperature = data.Temperature;
            Pressure = data.Pressure;
            MotorSpeed = data.MotorSpeed;
            IsSystemRunning = data.IsSystemRunning;
        }
    }
}
