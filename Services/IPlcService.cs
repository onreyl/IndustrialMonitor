using System;
using IndustrialMonitor.Models;

namespace IndustrialMonitor.Services
{
    /// <summary>
    /// Interface for PLC communication service.
    /// Enables dependency injection and unit testing.
    /// </summary>
    public interface IPlcService : IDisposable
    {
        // Connection properties
        string IpAddress { get; set; }
        bool IsConnected { get; }
        bool IsSimulationMode { get; }
        string ConnectionStatus { get; }

        // Events
        event EventHandler<string>? ConnectionStatusChanged;
        event EventHandler<ProcessData>? DataReceived;

        // Connection methods
        bool Connect();
        void Disconnect();

        // Read all process data at once (more efficient)
        ProcessData ReadAllData();

        // Write operations
        bool WriteSystemRunning(bool value);
        bool WriteTankLevel(float value);
    }
}
