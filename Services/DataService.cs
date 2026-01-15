using System;
using System.Windows.Threading;
using IndustrialMonitor.Models;

namespace IndustrialMonitor.Services
{
    /// <summary>
    /// Coordinates data flow between PLC and ViewModels.
    /// Handles polling timer and simulation logic.
    /// This separates the timing/polling concern from ViewModels.
    /// </summary>
    public class DataService : IDisposable
    {
        private readonly IPlcService _plcService;
        private readonly DispatcherTimer _timer;
        private readonly Random _random = new Random();
        private ProcessData _currentData;

        public event EventHandler<ProcessData>? DataUpdated;
        public event EventHandler<string>? StatusChanged;

        public ProcessData CurrentData => _currentData;
        public bool IsSimulationMode => _plcService.IsSimulationMode;

        public DataService(IPlcService plcService)
        {
            _plcService = plcService;
            _currentData = new ProcessData();

            _plcService.ConnectionStatusChanged += (s, msg) => StatusChanged?.Invoke(this, msg);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _timer.Tick += OnTimerTick;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public bool ConnectPlc()
        {
            return _plcService.Connect();
        }

        public void DisconnectPlc()
        {
            _plcService.Disconnect();
        }

        public void SetSystemRunning(bool running)
        {
            _currentData.IsSystemRunning = running;
            if (!_plcService.IsSimulationMode)
            {
                _plcService.WriteSystemRunning(running);
            }
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            if (_plcService.IsSimulationMode)
            {
                UpdateSimulation();
            }
            else
            {
                ReadFromPlc();
            }

            DataUpdated?.Invoke(this, _currentData);
        }

        private void UpdateSimulation()
        {
            if (_currentData.IsSystemRunning)
            {
                _currentData.TankLevel = Math.Clamp(
                    _currentData.TankLevel + (_random.NextDouble() * 2.0 - 1.0),
                    0, 100);

                _currentData.Temperature = Math.Clamp(
                    _currentData.Temperature + (_random.NextDouble() * 0.6 - 0.3),
                    20, 90);

                _currentData.Pressure = Math.Clamp(
                    _currentData.Pressure + (_random.NextDouble() * 0.1 - 0.05),
                    0, 6);

                _currentData.MotorSpeed = Math.Clamp(
                    _currentData.MotorSpeed + _random.Next(-20, 21),
                    1000, 2000);
            }
            else
            {
                // Motor slows down when stopped
                if (_currentData.MotorSpeed > 0)
                {
                    _currentData.MotorSpeed = Math.Max(0, _currentData.MotorSpeed - 50);
                }
            }
        }

        private void ReadFromPlc()
        {
            try
            {
                _currentData = _plcService.ReadAllData();
            }
            catch
            {
                // Connection lost, switch to simulation
                _plcService.Disconnect();
                StatusChanged?.Invoke(this, "PLC connection lost - Switched to Simulation");
            }
        }

        public void Dispose()
        {
            _timer.Stop();
            _plcService.Dispose();
        }
    }
}
