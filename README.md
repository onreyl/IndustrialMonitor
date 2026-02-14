# Industrial Monitor - PLC Monitoring System

A real-time industrial monitoring application for Siemens S7 PLCs built with WPF and MVVM architecture.

## Features

- Real-time PLC data monitoring (Temperature, Pressure, Status)
- Siemens S7 PLC communication via S7.Net library
- Simulation mode for development and testing
- Clean MVVM architecture with dependency injection
- Async/await for responsive UI

## Technology Stack

- .NET 10.0
- WPF (Windows Presentation Foundation)
- S7.Net Plus - Siemens S7 PLC communication
- Microsoft.Extensions.DependencyInjection

## Architecture

```
IndustrialMonitor/
├── Models/          # Data models (MachineStatus)
├── ViewModels/      # MVVM ViewModels with INotifyPropertyChanged
├── Views/           # XAML views and user controls
├── Services/        # Business logic (PlcService)
└── Resources/       # Styles and resources
```

## Setup

### Prerequisites
- Windows 10/11
- .NET 10.0 SDK
- Visual Studio 2022 or later

### Installation

1. Clone the repository
2. Open `IndustrialMonitor.sln` in Visual Studio
3. Restore NuGet packages
4. Build and run

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

## Usage

1. Click "Connect" to establish PLC connection
2. Use Control Panel to Start/Stop the process
3. Monitor real-time data in the dashboard
4. Click "Disconnect" to close connection

## Development Notes

- Simulation mode generates realistic sensor data for testing
- Polling interval: 1 second
- Supports async operations for non-blocking UI
- Extensible service layer for additional PLC operations

## Future Enhancements

- [ ] Data logging and historical trends
- [ ] Alarm management system
- [ ] Multi-PLC support
- [ ] Export data to CSV/Excel
- [ ] User authentication

## License

MIT License
