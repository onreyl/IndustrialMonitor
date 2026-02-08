
# IndustrialMonitor

A WPF-based desktop application designed for real-time monitoring and control of industrial process data via Siemens S7-1500 PLCs. The project is structured using the MVVM pattern and specifically optimized for testing with PLCSIM Advanced.

## Features

* **Live Data Tracking:** Monitor critical parameters including tank levels, temperature, pressure, and motor speed.
* **PLC Integration:** Uses `S7.Net` for direct communication with Siemens hardware.
* **Simulation Fallback:** Automatically switches to simulation mode if the PLC is unavailable, ensuring the UI remains functional for testing.
* **Command & Control:** Built-in support for toggling system states and updating PLC registers directly from the interface.

## Tech Stack

* **.NET 10.0** (WPF)
* **S7.Net 0.20.0** for S7 protocol communication
* **MVVM Architecture** for clean separation of concerns

## Getting Started

### Prerequisites
* Visual Studio 2022 or latest .NET SDK.
* Siemens TIA Portal.
* PLCSIM Advanced (recommended for local testing).

### Setup
1.  Clone the repository and restore dependencies:
    ```bash
    dotnet restore
    ```
2.  Configure your PLC IP address in `Services/PlcService.cs`. The default is set to `192.168.0.1`.
3.  Build and run the application.
4.  Click **"Connect PLC"** in the footer to initialize communication.

## PLC Configuration (DB1)

To interface with the application, create a **Global Data Block (DB1)** in TIA Portal with the following structure:

| Name | Data Type | Offset | Description |
| :--- | :--- | :--- | :--- |
| TankLevel | Real | 0.0 | Tank level (%) |
| Temperature | Real | 4.0 | Temperature (°C) |
| Pressure | Real | 8.0 | Pressure (bar) |
| MotorSpeed | DInt | 12.0 | Motor speed (rpm) |
| SystemRunning | Bool | 16.0 | System status |

> **Important:** Ensure "Optimized block access" is disabled for the Data Block and "Permit access with PUT/GET" is enabled in the CPU security settings.

## Project Structure

* **Core:** Base classes for MVVM (ObservableObject, RelayCommand).
* **Models:** Data contracts for process variables.
* **Services:** Logic for PLC connectivity and data polling.
* **ViewModels:** UI logic and state management.
* **Views:** XAML definitions for the user interface.

## License
Created for industrial automation testing and educational purposes.
