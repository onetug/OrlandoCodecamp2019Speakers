using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
