namespace IndustrialMonitor.Models
{
    /// <summary>
    /// Represents the process data from PLC or simulation.
    /// This is a pure data model with no logic.
    /// </summary>
    public class ProcessData
    {
        public double TankLevel { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public int MotorSpeed { get; set; }
        public bool IsSystemRunning { get; set; }

        public ProcessData()
        {
            TankLevel = 65.0;
            Temperature = 45.2;
            Pressure = 2.4;
            MotorSpeed = 1450;
            IsSystemRunning = false;
        }

        public ProcessData Clone()
        {
            return new ProcessData
            {
                TankLevel = this.TankLevel,
                Temperature = this.Temperature,
                Pressure = this.Pressure,
                MotorSpeed = this.MotorSpeed,
                IsSystemRunning = this.IsSystemRunning
            };
        }
    }
}
