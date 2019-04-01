using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class LedControl
    {
        private readonly DigitalOutputPort _outputPort;
        private readonly IBlockingTimer _blinkTimer;
        private bool _state;

        public LedControl(IDigitalPin ledPin, IBlockingTimer blinkTimer)
        {
            _outputPort = new DigitalOutputPort(ledPin, initialState: false);
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
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    _outputPort.State = value;
                }
            }
        }

        public void TurnOffLed()
        {
            State = false;
        }

        public void TurnOnLed()
        {
            State = true;
        }
    }
}
