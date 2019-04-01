using HardwareDrivers.LightSensors.APDS9301;
using HardwareDrivers.RPi;
using IoPinController.FileUtils;
using IoPinController.PinControllers.Linux;
using IoPinController.Utils;
using System;
using System.Threading;
using Utilities;

namespace RaspbianNetCoreDemo
{
    class Program
    {
        private const int LedPinNumber = 5;
        private const int LightSensorDeviceAddress = 0x39;
        private const float OnMinimumLuminosity = 100.0f;
        private const string I2cDevicePath = "/dev/i2c-1";

        static void Main()
        {
            //Compile with:
            //  dotnet publish ./RaspbianNetCoreDemo -c Release -r linux-arm --self-contained

            var taskSchedulerUtility = new TaskSchedulerUtility();
            var fileUtils = new AsyncFileUtil();

            var pinController = new LinuxPinController(fileUtils, taskSchedulerUtility);
            var ledPin = pinController.GetOrCreateOutputPin(LedPinNumber);

            BlockingTimer sleepTimer = new BlockingTimer(TimeSpan.FromMilliseconds(100));
            LedControl ledControl = new LedControl(ledPin, sleepTimer);

            //while (true)
            //{
            //    ledControl.Blink();
            //}
            
            var lightSensorDevice = new I2cDevice(I2cDevicePath, LightSensorDeviceAddress);
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

            //TODO: Dispose these somehow
            //      Only care if exception is thrown on startup when the I2C Bus and IO Pins are already setup I guess
            //lightSensor.Dispose();
            //pinController.Dispose();
        }
    }
}
