using System;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using Windows.System.Threading;
using Windows.Devices.I2c;

using HardwareDrivers.LightSensors.APDS9301;
using Utilities;

namespace Win10IoTDemo
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const int LedPinNumber = 5;
        private const int LightSensorDeviceAddress = 0x39;
        private const float OnMinimumLuminosity = 100.0f;

        private BackgroundTaskDeferral _deferral;
        private LedControl _ledController;

        private GpioPin _ledPin;
        private GpioController _gpioController;
        private APDS9301_LightSensor _lightSensor;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstance_Canceled;

            _gpioController = await GpioController.GetDefaultAsync();
            _ledPin = _gpioController.OpenPin(LedPinNumber);
            _ledPin.SetDriveMode(GpioPinDriveMode.Output);

            var sleepTimer = new BlockingTimer(TimeSpan.FromMilliseconds(100));
            _ledController = new LedControl(_ledPin, sleepTimer);

            var ledDeviceConnectionSettings = new I2cConnectionSettings(LightSensorDeviceAddress);

            var i2cController = await I2cController.GetDefaultAsync();
            var lightSensorDevice = i2cController.GetDevice(ledDeviceConnectionSettings);

            _lightSensor = new APDS9301_LightSensor(lightSensorDevice, APDS9301_LightSensor.MinimumPollingPeriod);

            ThreadPoolTimer.CreatePeriodicTimer(Tick, TimeSpan.FromMilliseconds(500));
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (_deferral != null)
            {
                _deferral.Complete();
                _deferral = null;
            }
        }

        private void Tick(ThreadPoolTimer timer)
        {
            float currentLuminosity = _lightSensor.Luminosity;

            if (!_ledController.State && currentLuminosity <= OnMinimumLuminosity)
            {
                _ledController.Blink();
                _ledController.Blink();
                _ledController.TurnOnLed();
                System.Diagnostics.Debug.WriteLine(currentLuminosity.ToString());
            }
            else if (_ledController.State && currentLuminosity > OnMinimumLuminosity)
            {
                _ledController.TurnOffLed();
                System.Diagnostics.Debug.WriteLine(currentLuminosity.ToString());
            }
        }
    }
}
