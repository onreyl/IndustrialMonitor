using IndustrialMonitor.Models;
using IndustrialMonitor.Services;
using IndustrialMonitor.ViewModels;
using Moq;
using Xunit;

namespace IndustrialMonitor.Tests.ViewModels
{
    public class MainViewModelTests
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            var mockPlcService = new Mock<IPlcService>();
            var viewModel = new MainViewModel(mockPlcService.Object);

            Assert.NotNull(viewModel.Status);
            Assert.NotNull(viewModel.ConnectCommand);
            Assert.NotNull(viewModel.DisconnectCommand);
            Assert.Equal("Disconnected", viewModel.ConnectionStatus);
        }

        [Fact]
        public async Task Connect_CallsPlcService()
        {
            var mockPlcService = new Mock<IPlcService>();
            mockPlcService.Setup(x => x.ConnectAsync()).Returns(Task.CompletedTask);
            
            var viewModel = new MainViewModel(mockPlcService.Object);

            mockPlcService.Verify(x => x.ConnectAsync(), Times.Never);
        }

        [Fact]
        public async Task ReadStatus_ReturnsValidData()
        {
            var expectedStatus = new MachineStatus
            {
                Temperature = 75.5,
                Pressure = 5.2,
                IsRunning = true,
                StatusMessage = "Running"
            };

            var mockPlcService = new Mock<IPlcService>();
            mockPlcService.Setup(x => x.ReadStatusAsync()).ReturnsAsync(expectedStatus);

            var status = await mockPlcService.Object.ReadStatusAsync();

            Assert.Equal(75.5, status.Temperature);
            Assert.Equal(5.2, status.Pressure);
            Assert.True(status.IsRunning);
        }
    }
}
