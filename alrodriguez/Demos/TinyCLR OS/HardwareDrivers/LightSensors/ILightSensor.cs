using System;

namespace HardwareDrivers.LightSensors
{
    public interface ILightSensor : IDisposable
    {
        /// <summary>
        /// Gets the last read value from the light sensor. The higher the number to brighter.
        /// </summary>
        float Luminosity { get; }
    }
}
