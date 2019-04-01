using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace IoPinController
{
    public interface IPinController : IDisposable
    {
        Encoding DefaultFileEncoding { get; }

        IImmutableDictionary<int, Pin> ConfiguredPins { get; }

        void SetPinDirectionToInput(int pinNumber);
        void SetPinDirectionToOutput(int pinNumber);
        void SetPinDirectionToStopped(int pinNumber);
    }
}
