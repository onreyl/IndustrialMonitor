using IndustrialMonitor.Models;
using System.Threading.Tasks;

namespace IndustrialMonitor.Services
{
    /// <summary>
    /// Defines the contract for PLC communication services.
    /// </summary>
    public interface IPlcService
    {
        Task ConnectAsync();
        void Disconnect();
        Task StartProcessAsync();
        Task StopProcessAsync();
        Task<MachineStatus> ReadStatusAsync();
    }
}
