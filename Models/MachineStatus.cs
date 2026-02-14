namespace IndustrialMonitor.Models
{
    /// <summary>
    /// Represents the operational status of an industrial machine.
    /// </summary>
    public class MachineStatus
    {
        /// <summary>
        /// Gets or sets the current temperature reading in degrees.
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Gets or sets the current pressure reading.
        /// </summary>
        public double Pressure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the machine is currently operational.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Gets or sets the current status message describing the machine state.
        /// </summary>
        public string StatusMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the last status update.
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
