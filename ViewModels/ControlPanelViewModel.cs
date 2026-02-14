using IndustrialMonitor.Models;
using IndustrialMonitor.Services;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// ViewModel for the control panel, handling manual process control and status display.
    /// </summary>
    public class ControlPanelViewModel : BaseViewModel
    {
        private readonly PlcService _plcService;
        private double _tankLevel;
        private double _temperature;
        private double _pressure;
        private int _motorSpeed;
        private string _systemStatusText = "SYSTEM READY";
        private Brush _systemStatusColor = Brushes.Orange;

        public ControlPanelViewModel(PlcService plcService)
        {
            _plcService = plcService;
            StartCommand = new RelayCommand(async _ => await StartProcess());
            StopCommand = new RelayCommand(async _ => await StopProcess());
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public double TankLevel { get => _tankLevel; set => SetProperty(ref _tankLevel, value); }
        public double Temperature { get => _temperature; set => SetProperty(ref _temperature, value); }
        public double Pressure { get => _pressure; set => SetProperty(ref _pressure, value); }
        public int MotorSpeed { get => _motorSpeed; set => SetProperty(ref _motorSpeed, value); }
        public string SystemStatusText { get => _systemStatusText; set => SetProperty(ref _systemStatusText, value); }
        public Brush SystemStatusColor { get => _systemStatusColor; set => SetProperty(ref _systemStatusColor, value); }

        private async Task StartProcess()
        {
            await _plcService.StartProcessAsync();
            UpdateStatusUI(true);
        }

        private async Task StopProcess()
        {
            await _plcService.StopProcessAsync();
            UpdateStatusUI(false);
        }

        private void UpdateStatusUI(bool isRunning)
        {
            if (isRunning)
            {
                SystemStatusText = "RUNNING";
                SystemStatusColor = Brushes.LightGreen;
            }
            else
            {
                SystemStatusText = "STOPPED";
                SystemStatusColor = Brushes.IndianRed;
            }
        }

        /// <summary>
        /// Updates the view model properties based on the latest machine status.
        /// </summary>
        /// <param name="status">The latest status from the PLC.</param>
        public void UpdateFromStatus(MachineStatus status)
        {
            Temperature = status.Temperature;
            Pressure = status.Pressure;

            // Simulate extra sensor data for the UI
            TankLevel = status.IsRunning ? 45 + new Random().NextDouble() * 10 : 10;
            MotorSpeed = status.IsRunning ? 1450 + new Random().Next(0, 100) : 0;

            UpdateStatusUI(status.IsRunning);
        }
    }
}
