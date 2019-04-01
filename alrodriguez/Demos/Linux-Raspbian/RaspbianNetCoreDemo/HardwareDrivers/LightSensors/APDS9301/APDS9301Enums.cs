using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareDrivers.LightSensors.APDS9301
{
    public enum IntegrationTiming : byte
    {
        Ms13 = 0,
        Ms101,
        Ms402,
        Manual
    }

    public enum Gain
    {
        Low = 0,
        High = 1
    }

    public enum InterruptMode : byte
    {
        Disable = 0,
        Enable
    }
}
