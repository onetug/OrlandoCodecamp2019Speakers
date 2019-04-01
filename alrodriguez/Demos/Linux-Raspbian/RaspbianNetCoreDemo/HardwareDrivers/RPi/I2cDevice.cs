using HardwareDrivers.RPi.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareDrivers.RPi
{
    public class I2cDevice : IDisposable
    {
        private readonly int _busHandle;
        private readonly int _address;

        public I2cDevice(string i2cBusPath, int address)
        {
            _address = address;
            _busHandle = I2CWrapper.OpenBus(i2cBusPath);
        }
        
        public void Write(byte[] output)
        {
            I2CWrapper.WriteBytes(_busHandle, _address, output, output.Length);
        }

        public void WriteRead(byte[] output, byte[] input)
        {
            I2CWrapper.WriteBytes(_busHandle, _address, output, output.Length);
            I2CWrapper.ReadBytes(_busHandle, _address, input, input.Length);
        }

        public void Dispose()
        {
            I2CWrapper.CloseBus(_busHandle);
        }
    }
}
