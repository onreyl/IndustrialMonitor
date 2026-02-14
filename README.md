# Industrial Monitor - PLC Monitoring System

A real-time industrial monitoring application for Siemens S7 PLCs built with WPF and MVVM architecture.

## Features

- Real-time PLC data monitoring (Temperature, Pressure, Status)
- Siemens S7 PLC communication via S7.Net library
- Simulation mode for development and testing
- Clean MVVM architecture with dependency injection
- Async/await for responsive UI
- Unit tests with xUnit and Moq
- Interface-based design for testability

## Technology Stack

- .NET 10.0
- WPF (Windows Presentation Foundation)
- S7.Net Plus 0.20.0 - Siemens S7 PLC communication
- Microsoft.Extensions.DependencyInjection
- xUnit & Moq for testing

## Architecture

```
IndustrialMonitor/
├── Models/          # Data models (MachineStatus)
├── ViewModels/      # MVVM ViewModels with INotifyPropertyChanged
├── Views/           # XAML views and user controls
├── Services/        # Business logic (IPlcService, PlcService)
├── Resources/       # Styles and resources
└── Tests/           # Unit tests
```

## Setup

### Prerequisites
- Windows 10/11
- .NET 10.0 SDK
- Visual Studio 2022 or later
- Siemens TIA Portal (optional, for real PLC)
- PLCSIM Advanced (optional, for simulation)

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/onreyl/IndustrialMonitor.git
   ```
2. Open `IndustrialMonitor.sln` in Visual Studio
3. Restore NuGet packages
   ```bash
   dotnet restore
   ```
4. Build and run
   ```bash
   dotnet build
   dotnet run
   ```

### PLC Configuration

For real PLC connection, configure in `App.xaml.cs`:
```csharp
var plcService = new PlcService(
    CpuType.S71200,
    "192.168.0.1",  // PLC IP
    0,              // Rack
    1,              // Slot
    isSimulation: false
);
```

### Simulation Mode

By default, the application runs in simulation mode (no physical PLC required):
```csharp
isSimulation: true
```

## PLC Data Block Structure (DB1)

For real PLC integration, create a Global Data Block in TIA Portal:

| Name | Data Type | Offset | Description |
| :--- | :--- | :--- | :--- |
| Temperature | Real | 0.0 | Temperature (°C) |
| Pressure | Real | 4.0 | Pressure (bar) |
| IsRunning | Bool | 8.0 | System status |
| StatusMessage | String[50] | 10.0 | Status text |

> **Note:** Disable "Optimized block access" and enable "Permit access with PUT/GET" in CPU settings.

## Usage

1. Click "Connect" to establish PLC connection
2. Use Control Panel to Start/Stop the process
3. Monitor real-time data in the dashboard
4. Click "Disconnect" to close connection

## Running Tests

```bash
cd Tests
dotnet test
```

## Development Notes

- Simulation mode generates realistic sensor data for testing
- Polling interval: 1 second (configurable in appsettings.json)
- Supports async operations for non-blocking UI
- Interface-based design (IPlcService) for easy mocking and testing
- Comprehensive error handling with user feedback

## Future Enhancements

- [ ] Data logging and historical trends
- [ ] Alarm management system
- [ ] Multi-PLC support
- [ ] Export data to CSV/Excel
- [ ] User authentication
- [ ] Real-time charting

## License

MIT License
