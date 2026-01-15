using System;

namespace IndustrialMonitor.Core
{
    /// <summary>
    /// Simple messenger for communication between ViewModels.
    /// This is a lightweight alternative to EventAggregator/MediatR.
    /// </summary>
    public class Messenger
    {
        private static readonly Lazy<Messenger> _instance = new Lazy<Messenger>(() => new Messenger());
        public static Messenger Default => _instance.Value;

        // Events for different message types
        public event EventHandler<SystemStateChangedMessage>? SystemStateChanged;
        public event EventHandler<ProcessDataUpdatedMessage>? ProcessDataUpdated;
        public event EventHandler<PlcConnectionChangedMessage>? PlcConnectionChanged;

        private Messenger() { }

        public void Send(SystemStateChangedMessage message)
        {
            SystemStateChanged?.Invoke(this, message);
        }

        public void Send(ProcessDataUpdatedMessage message)
        {
            ProcessDataUpdated?.Invoke(this, message);
        }

        public void Send(PlcConnectionChangedMessage message)
        {
            PlcConnectionChanged?.Invoke(this, message);
        }
    }

    #region Message Types

    public class SystemStateChangedMessage
    {
        public bool IsRunning { get; }
        public SystemStateChangedMessage(bool isRunning)
        {
            IsRunning = isRunning;
        }
    }

    public class ProcessDataUpdatedMessage
    {
        public double TankLevel { get; }
        public double Temperature { get; }
        public double Pressure { get; }
        public int MotorSpeed { get; }

        public ProcessDataUpdatedMessage(double tankLevel, double temperature, double pressure, int motorSpeed)
        {
            TankLevel = tankLevel;
            Temperature = temperature;
            Pressure = pressure;
            MotorSpeed = motorSpeed;
        }
    }

    public class PlcConnectionChangedMessage
    {
        public bool IsConnected { get; }
        public bool IsSimulationMode { get; }
        public string Status { get; }

        public PlcConnectionChangedMessage(bool isConnected, bool isSimulationMode, string status)
        {
            IsConnected = isConnected;
            IsSimulationMode = isSimulationMode;
            Status = status;
        }
    }

    #endregion
}
