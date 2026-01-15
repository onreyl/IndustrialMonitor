using System.Windows.Media;
using IndustrialMonitor.Core;
using IndustrialMonitor.Models;
using IndustrialMonitor.Services;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// ViewModel for the Process Overview (P&ID) view.
    /// Displays graphical representation of process.
    /// </summary>
    public class ProcessOverviewViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        #region Properties

        private double _tankLevel;
        public double TankLevel
        {
            get => _tankLevel;
            set
            {
                if (SetProperty(ref _tankLevel, value))
                {
                    OnPropertyChanged(nameof(ScaledTankLevel));
                }
            }
        }

        /// <summary>
        /// Tank level scaled for visual display (0-100% -> 0-140px)
        /// </summary>
        public double ScaledTankLevel => TankLevel * 1.4;

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

        private bool _isMotorRunning;
        public bool IsMotorRunning
        {
            get => _isMotorRunning;
            set
            {
                if (SetProperty(ref _isMotorRunning, value))
                {
                    OnPropertyChanged(nameof(MotorStatusColor));
                }
            }
        }

        public Brush MotorStatusColor => IsMotorRunning ? Brushes.LimeGreen : Brushes.Red;

        #endregion

        public ProcessOverviewViewModel(DataService dataService)
        {
            _dataService = dataService;

            // Subscribe to data updates
            _dataService.DataUpdated += OnDataUpdated;

            // Subscribe to system state changes from other ViewModels
            Messenger.Default.SystemStateChanged += OnSystemStateChanged;

            // Initialize with current data
            UpdateFromProcessData(_dataService.CurrentData);
        }

        private void OnDataUpdated(object? sender, ProcessData data)
        {
            UpdateFromProcessData(data);
        }

        private void OnSystemStateChanged(object? sender, SystemStateChangedMessage message)
        {
            IsMotorRunning = message.IsRunning;
        }

        private void UpdateFromProcessData(ProcessData data)
        {
            TankLevel = data.TankLevel;
            Temperature = data.Temperature;
            Pressure = data.Pressure;
            MotorSpeed = data.MotorSpeed;
            IsMotorRunning = data.IsSystemRunning;
        }
    }
}
