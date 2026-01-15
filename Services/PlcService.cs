using System;
using S7.Net;
using IndustrialMonitor.Models;

namespace IndustrialMonitor.Services
{
    /// <summary>
    /// Service for managing Siemens S7 PLC connections.
    /// Implements IPlcService for dependency injection.
    /// Falls back to simulation mode if PLC is not available.
    /// </summary>
    public class PlcService : IPlcService
    {
        private Plc? _plc;
        private bool _isSimulationMode = true;
        private string _connectionStatus = "Disconnected";

        // PLC Configuration
        public string IpAddress { get; set; } = "192.168.0.1";
        public CpuType CpuType { get; set; } = CpuType.S71500;
        public short Rack { get; set; } = 0;
        public short Slot { get; set; } = 1;

        // Data Block Configuration
        public int DataBlock { get; set; } = 1;

        // Tag Offsets
        public int TankLevelOffset { get; set; } = 0;
        public int TemperatureOffset { get; set; } = 4;
        public int PressureOffset { get; set; } = 8;
        public int MotorSpeedOffset { get; set; } = 12;
        public int SystemRunningOffset { get; set; } = 16;

        // Interface Properties
        public bool IsConnected => _plc?.IsConnected ?? false;
        public bool IsSimulationMode => _isSimulationMode;
        public string ConnectionStatus => _connectionStatus;

        // Events
        public event EventHandler<string>? ConnectionStatusChanged;
        public event EventHandler<ProcessData>? DataReceived;

        public bool Connect()
        {
            try
            {
                _connectionStatus = "Connecting...";
                ConnectionStatusChanged?.Invoke(this, _connectionStatus);

                _plc = new Plc(CpuType, IpAddress, Rack, Slot);
                _plc.Open();

                if (_plc.IsConnected)
                {
                    _isSimulationMode = false;
                    _connectionStatus = $"Connected to PLC at {IpAddress}";
                    ConnectionStatusChanged?.Invoke(this, _connectionStatus);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _connectionStatus = $"Connection failed: {ex.Message}";
                ConnectionStatusChanged?.Invoke(this, _connectionStatus);
            }

            _isSimulationMode = true;
            _connectionStatus = "Simulation Mode (PLC not available)";
            ConnectionStatusChanged?.Invoke(this, _connectionStatus);
            return false;
        }

        public void Disconnect()
        {
            if (_plc?.IsConnected == true)
            {
                _plc.Close();
            }
            _isSimulationMode = true;
            _connectionStatus = "Disconnected";
            ConnectionStatusChanged?.Invoke(this, _connectionStatus);
        }

        public ProcessData ReadAllData()
        {
            var data = new ProcessData();

            if (!IsConnected) return data;

            try
            {
                data.TankLevel = Convert.ToDouble(_plc!.Read(DataType.DataBlock, DataBlock, TankLevelOffset, VarType.Real, 1));
                data.Temperature = Convert.ToDouble(_plc.Read(DataType.DataBlock, DataBlock, TemperatureOffset, VarType.Real, 1));
                data.Pressure = Convert.ToDouble(_plc.Read(DataType.DataBlock, DataBlock, PressureOffset, VarType.Real, 1));
                data.MotorSpeed = Convert.ToInt32(_plc.Read(DataType.DataBlock, DataBlock, MotorSpeedOffset, VarType.DInt, 1));

                var runningObj = _plc.Read(DataType.DataBlock, DataBlock, SystemRunningOffset, VarType.Bit, 1, 0);
                data.IsSystemRunning = runningObj != null && (bool)runningObj;

                DataReceived?.Invoke(this, data);
            }
            catch (Exception)
            {
                // Log error in real application
            }

            return data;
        }

        public bool WriteSystemRunning(bool value)
        {
            if (!IsConnected) return false;
            try
            {
                _plc!.Write(DataType.DataBlock, DataBlock, SystemRunningOffset, value, 0);
                return true;
            }
            catch { return false; }
        }

        public bool WriteTankLevel(float value)
        {
            if (!IsConnected) return false;
            try
            {
                _plc!.Write(DataType.DataBlock, DataBlock, TankLevelOffset, value);
                return true;
            }
            catch { return false; }
        }

        public void Dispose()
        {
            Disconnect();
            _plc = null;
        }
    }
}
