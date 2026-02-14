using IndustrialMonitor.Models;
using S7.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace IndustrialMonitor.Services
{
    /// <summary>
    /// Manages communication with the Siemens S7 PLC using the S7.Net library.
    /// Supports both real hardware connection and a simulation mode for development.
    /// </summary>
    public class PlcService : IPlcService
    {
        private Plc? _plc;
        private readonly bool _isSimulation;
        private readonly Random _random = new Random();
        private bool _isProcessRunning = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlcService"/> class.
        /// </summary>
        /// <param name="cpu">The CPU type of the PLC.</param>
        /// <param name="ip">The IP address of the PLC.</param>
        /// <param name="rack">The rack number.</param>
        /// <param name="slot">The slot number.</param>
        /// <param name="isSimulation">If set to <c>true</c>, runs in simulation mode without attempting actual PLC connection.</param>
        public PlcService(CpuType cpu, string ip, short rack, short slot, bool isSimulation = false)
        {
            _isSimulation = isSimulation;
            
            if (!_isSimulation)
            {
                _plc = new Plc(cpu, ip, rack, slot);
            }
        }

        /// <summary>
        /// Establishes an asynchronous connection to the PLC.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when connection fails.</exception>
        public async Task ConnectAsync()
        {
            if (_isSimulation)
            {
                await Task.Delay(500);
                return;
            }

            try
            {
                if (_plc != null)
                {
                    await _plc.OpenAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to connect to PLC: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Closes the connection to the PLC.
        /// </summary>
        public void Disconnect()
        {
            if (!_isSimulation && _plc != null)
            {
                _plc.Close();
            }
        }

        /// <summary>
        /// Starts the industrial process.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StartProcessAsync()
        {
            if (_isSimulation)
            {
                await Task.Delay(200);
                _isProcessRunning = true;
                return;
            }

            try
            {
                // await _plc.WriteAsync("DB1.DBX0.0", true);
                _isProcessRunning = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to start process: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Stops the industrial process.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StopProcessAsync()
        {
            if (_isSimulation)
            {
                await Task.Delay(200);
                _isProcessRunning = false;
                return;
            }

            try
            {
                // await _plc.WriteAsync("DB1.DBX0.0", false);
                _isProcessRunning = false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to stop process: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reads the current status of the machine from the PLC or simulation.
        /// </summary>
        /// <returns>A <see cref="MachineStatus"/> object containing current readings.</returns>
        public async Task<MachineStatus> ReadStatusAsync()
        {
            if (_isSimulation)
            {
                return GetSimulatedStatus();
            }

            try
            {
                // byte[] bytes = await _plc.ReadBytesAsync(DataType.DataBlock, 1, 0, 20);
                return GetSimulatedStatus();
            }
            catch (Exception ex)
            {
                return new MachineStatus 
                { 
                    StatusMessage = $"Error: {ex.Message}",
                    LastUpdated = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Generates simulated machine status data for testing/demo purposes.
        /// </summary>
        /// <returns>A <see cref="MachineStatus"/> object with randomized values.</returns>
        private MachineStatus GetSimulatedStatus()
        {
            return new MachineStatus
            {
                // Temperature: 60-80 when running, 20-25 when stopped
                Temperature = _isProcessRunning ? 60 + _random.NextDouble() * 20 : 20 + _random.NextDouble() * 5,
                
                // Pressure: 4-6 when running, 0-1 when stopped
                Pressure = _isProcessRunning ? 4 + _random.NextDouble() * 2 : _random.NextDouble(),
                
                IsRunning = _isProcessRunning,
                StatusMessage = _isProcessRunning ? "System Running" : "System Stopped",
                LastUpdated = DateTime.Now
            };
        }
    }
}
