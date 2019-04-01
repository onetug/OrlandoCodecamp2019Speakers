using IoPinController;
using IoPinController.PinControllers.Linux;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class LedControl
    {
        private readonly LinuxOutputPin _ledPin;
        private readonly IBlockingTimer _blinkTimer;

        public LedControl(LinuxOutputPin ledPin, IBlockingTimer blinkTimer)
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
            _ledPin.SetOutputModeAsync(OutputModeType.Low);
        }

        public void TurnOnLed()
        {
            State = true;
            _ledPin.SetOutputModeAsync(OutputModeType.High);
        }
    }
}
