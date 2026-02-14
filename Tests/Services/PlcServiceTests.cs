using IndustrialMonitor.Services;
using S7.Net;
using Xunit;

namespace IndustrialMonitor.Tests.Services
{
    public class PlcServiceTests
    {
        [Fact]
        public async Task ConnectAsync_InSimulationMode_CompletesSuccessfully()
        {
            var service = new PlcService(CpuType.S71200, "192.168.0.1", 0, 1, isSimulation: true);

            await service.ConnectAsync();

            Assert.True(true);
        }

        [Fact]
        public async Task StartProcessAsync_SetsProcessRunning()
        {
            var service = new PlcService(CpuType.S71200, "192.168.0.1", 0, 1, isSimulation: true);
            
            await service.StartProcessAsync();
            var status = await service.ReadStatusAsync();

            Assert.True(status.IsRunning);
            Assert.Contains("Running", status.StatusMessage);
        }

        [Fact]
        public async Task StopProcessAsync_SetsProcessStopped()
        {
            var service = new PlcService(CpuType.S71200, "192.168.0.1", 0, 1, isSimulation: true);
            
            await service.StartProcessAsync();
            await service.StopProcessAsync();
            var status = await service.ReadStatusAsync();

            Assert.False(status.IsRunning);
            Assert.Contains("Stopped", status.StatusMessage);
        }

        [Fact]
        public async Task ReadStatusAsync_ReturnsValidData()
        {
            var service = new PlcService(CpuType.S71200, "192.168.0.1", 0, 1, isSimulation: true);
            
            var status = await service.ReadStatusAsync();

            Assert.NotNull(status);
            Assert.True(status.Temperature >= 0);
            Assert.True(status.Pressure >= 0);
            Assert.NotNull(status.StatusMessage);
        }
    }
}
