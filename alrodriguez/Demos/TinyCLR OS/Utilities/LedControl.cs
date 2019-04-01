using System;
using System.Collections;
using System.Text;
using System.Threading;
using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Utilities
{
    public class LedControl
    {
        private readonly GpioPin _ledPin;
        private readonly IBlockingTimer _blinkTimer;

        public LedControl(GpioPin ledPin, IBlockingTimer blinkTimer)
        {
            _ledPin = ledPin;
            _blinkTimer = blinkTimer;
        }

        public void Blink()
        {
            TurnOnLed();
            _blinkTimer.Run();

            TurnOffLed();
            _blinkTimer.Run();
        }

        public bool State
        {
            get;
            private set;
        }

        public void TurnOffLed()
        {
            State = false;
            _ledPin.Write(GpioPinValue.Low);
        }

        public void TurnOnLed()
        {
            State = true;
            _ledPin.Write(GpioPinValue.High);
        }
    }
}
