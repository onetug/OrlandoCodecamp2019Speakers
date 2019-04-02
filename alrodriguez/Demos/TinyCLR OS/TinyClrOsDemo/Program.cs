using System;
using System.Threading;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.I2c;
using GHIElectronics.TinyCLR.Devices.I2c.Provider;
using HardwareDrivers.LightSensors.APDS9301;
using Utilities;
using FezPins = GHIElectronics.TinyCLR.Pins.FEZ;

namespace TinyCLRApplicationSample
{
    class Program
    {
        private const float OnMinimumLuminosity = 100.0f;

        static void Main()
        {
            GpioPin led = GpioController.GetDefault().OpenPin(FezPins.GpioPin.D0);
            led.SetDriveMode(GpioPinDriveMode.Output);

            BlockingTimer sleepTimer = new BlockingTimer(TimeSpan.FromMilliseconds(100));
            LedControl ledControl = new LedControl(led, sleepTimer);

            int sdaPin = FezPins.GpioPin.A0;
            int slcPin = FezPins.GpioPin.A1;
            int lightSensorDeviceAddress = 0x39;
            I2cConnectionSettings ledDeviceConnectionSettings = new I2cConnectionSettings(lightSensorDeviceAddress, I2cAddressFormat.SevenBit, I2cBusSpeed.StandardMode);

            I2cControllerSoftwareProvider i2cProvider = new I2cControllerSoftwareProvider(sdaPin, slcPin, false);
            I2cController i2cController = I2cController.FromProvider(i2cProvider);
            I2cDevice lightSensorDevice = i2cController.GetDevice(ledDeviceConnectionSettings);

            var lightSensor = new APDS9301_LightSensor(lightSensorDevice, APDS9301_LightSensor.MinimumPollingPeriod);

            while (true)
            {
                float currentLuminosity = lightSensor.Luminosity;

                if (!ledControl.State && currentLuminosity <= OnMinimumLuminosity)
                {
                    ledControl.Blink();
                    ledControl.Blink();
                    ledControl.TurnOnLed();
                    System.Diagnostics.Debug.WriteLine(currentLuminosity.ToString());
                }
                else if (ledControl.State && currentLuminosity > OnMinimumLuminosity)
                {
                    ledControl.TurnOffLed();
                    System.Diagnostics.Debug.WriteLine(currentLuminosity.ToString());
                }

                Thread.Sleep(10);
            }
        }
    }
}
