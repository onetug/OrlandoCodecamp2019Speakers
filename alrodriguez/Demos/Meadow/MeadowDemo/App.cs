using HardwareDrivers.LightSensors.APDS9301;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace MeadowDemo
{
    public class App : AppBase<F7Micro, App>
    {
        private const string LedPinName = "5";
        private const ushort LightSensorDeviceAddress = 0x39;
        private const float OnMinimumLuminosity = 100.0f;

        //TODO: Find out the real number
        private const int I2cDeviceClockHz = 100;

        private readonly BlockingTimer _sleepTimer;
        private readonly LedControl _ledControl;

        private readonly APDS9301_LightSensor _lightSensor;

        public App()
        {
            //TODO: Find out if this is even right for finding the pin
            var ledPin = (IDigitalPin)Device.Pins.AllPins.Single(x => x.Name == LedPinName);

            _sleepTimer = new BlockingTimer(TimeSpan.FromMilliseconds(100));
            _ledControl = new LedControl(ledPin, _sleepTimer);

            var lightSensorI2cConfig = new I2cPeripheral.Configuration(LightSensorDeviceAddress, I2cDeviceClockHz);
            var lightI2cPeripheral = new I2cPeripheral(lightSensorI2cConfig);

            _lightSensor = new APDS9301_LightSensor(lightI2cPeripheral, APDS9301_LightSensor.MinimumPollingPeriod);
        }

        public void Run()
        {
            while (true)
            {
                float currentLuminosity = _lightSensor.Luminosity;

                if (!_ledControl.State && currentLuminosity <= OnMinimumLuminosity)
                {
                    _ledControl.Blink();
                    _ledControl.Blink();
                    _ledControl.TurnOnLed();
                    System.Diagnostics.Debug.WriteLine(currentLuminosity.ToString());
                }
                else if (_ledControl.State && currentLuminosity > OnMinimumLuminosity)
                {
                    _ledControl.TurnOffLed();
                    System.Diagnostics.Debug.WriteLine(currentLuminosity.ToString());
                }

                Thread.Sleep(10);
            }
        }
    }
}

