using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace HardwareDrivers.LightSensors.APDS9301
{
    public class APDS9301_LightSensor
    {
        /// <summary>
        /// Minimum value that should be used for the polling frequency.
        /// </summary>
        public static readonly TimeSpan MinimumPollingPeriod = TimeSpan.FromMilliseconds(100);

        private static class Registers
        {
            public const byte Control = 0x80;
            public const byte Timing = 0x81;
            public const byte ID = 0x8A;
            public const byte Data0 = 0x8C;
            public const byte Data1 = 0x8E;
            public const byte Interrupt = 0x86;
        }

        private readonly ushort _updateInterval = 100;
        private readonly I2cDevice _ledDevice;
        private readonly I2CHelper _i2cHelper;

        private readonly Thread _updateThread;
        public APDS9301_LightSensor(I2cDevice ledDevice, TimeSpan updateInterval)
        {
            if (updateInterval.TotalMilliseconds > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(updateInterval), $"Update Interval must be less than {ushort.MaxValue} milliseconds");
            }
            else if (updateInterval.TotalMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(updateInterval), $"Update Interval must be greater than 0 but it's {updateInterval.TotalMilliseconds} milliseconds");
            }

            _updateInterval = (ushort)updateInterval.TotalMilliseconds;

            _ledDevice = ledDevice;
            _i2cHelper = new I2CHelper(_ledDevice);

            TurnOn();
            SensorGain = Gain.Low;
            Timing = IntegrationTiming.Ms13;
            DisableInterrupt();

            // Wait for the sensor to prepare the first reading (402ms after power on).
            Thread.Sleep(410);
            _updateThread = new Thread(() =>
            {
                while (true)
                {
                    UpdateLumosity();
                    Thread.Sleep(_updateInterval);
                }
            });
            _updateThread.Start();
        }


        public float Luminosity
        {
            get;
            private set;
        }

        public Gain SensorGain
        {
            get
            {
                byte data = (byte)(_i2cHelper.ReadByteFromRegister(Registers.Timing) & 0x10);
                return data == 0 ? Gain.Low : Gain.High;
            }
            set
            {
                int data = _i2cHelper.ReadByteFromRegister(Registers.Timing);
                if (value == Gain.Low)
                {
                    data &= ~0x10;
                }
                else
                {
                    data |= 0x10;
                }
                _i2cHelper.WriteByteToRegister(Registers.Timing, (byte)data);
            }
        }

        public IntegrationTiming Timing
        {
            get
            {
                byte timing = _i2cHelper.ReadByteFromRegister(Registers.Timing);
                timing &= 0x03;

                if (timing == 0x00)
                {
                    return IntegrationTiming.Ms13;
                }
                else if (timing == 0x01)
                {
                    return IntegrationTiming.Ms101;
                }
                else
                {
                    return IntegrationTiming.Ms402;
                }
            }
            set
            {
                int timing = _i2cHelper.ReadByteFromRegister(Registers.Timing);
                timing &= ~0x03;
                if (value == IntegrationTiming.Ms13)
                {
                    timing |= 0x00;
                }
                else if (value == IntegrationTiming.Ms101)
                {
                    timing |= 0x01;
                }
                else if (value == IntegrationTiming.Ms402)
                {
                    timing |= 0x02;
                }
                _i2cHelper.WriteByteToRegister(Registers.Timing, (byte)timing);
            }
        }

        public void Dispose()
        {
            TurnOff();
            if (_updateThread.IsAlive)
            {
                _updateThread.Abort();
            }
        }

        public void TurnOff()
        {
            _i2cHelper.WriteByteToRegister(Registers.Control, 0x00);
        }

        public void TurnOn()
        {
            _i2cHelper.WriteByteToRegister(Registers.Control, 0x03);
        }

        private void DisableInterrupt()
        {
            int interruptVal = _i2cHelper.ReadByteFromRegister(Registers.Interrupt);
            interruptVal &= ~0x30;

            _i2cHelper.WriteByteToRegister(Registers.Interrupt, (byte)interruptVal);
        }

        private void UpdateLumosity()
        {
            ushort ch0Int = _i2cHelper.ReadUShort(Registers.Data0, ByteOrder.LittleEndian);
            ushort ch1Int = _i2cHelper.ReadUShort(Registers.Data1, ByteOrder.LittleEndian);

            if (ch0Int == 0 || ch1Int == 0)
            {
                Luminosity = 0.0f;
                return;
            }

            float ch0 = ch0Int;
            float ch1 = ch1Int;
            IntegrationTiming timing = Timing;
            switch (timing)
            {
                case IntegrationTiming.Ms13:

                    if ((ch1Int >= 5047) || (ch0Int >= 5047))
                    {
                        Luminosity = (float)(1.0 / 0.0);
                        return;
                    }
                    break;
                case IntegrationTiming.Ms101:
                    if ((ch1Int >= 37177) || (ch0Int >= 37177))
                    {
                        Luminosity = (float)(1.0 / 0.0);
                        return;
                    }
                    break;
                case IntegrationTiming.Ms402:
                    if ((ch1Int >= 65535) || (ch0Int >= 65535))
                    {
                        Luminosity = (float)(1.0 / 0.0);
                        return;
                    }
                    break;
            }

            float ratio = ch1 / ch0;
            switch (timing)
            {
                case IntegrationTiming.Ms13:
                    ch0 *= (float)(1 / 0.034);
                    ch1 *= (float)(1 / 0.034);
                    break;
                case IntegrationTiming.Ms101:
                    ch0 *= (float)(1 / 0.252);
                    ch1 *= (float)(1 / 0.252);
                    break;
                case IntegrationTiming.Ms402:
                    ch0 *= 1;
                    ch1 *= 1;
                    break;
            }

            if (SensorGain == Gain.Low)
            {
                ch0 *= 16;
                ch1 *= 16;
            }

            float luxVal = 0.0f;
            if (ratio <= 0.5)
            {
                luxVal = (float)(0.0304 * ch0) - (float)((0.062 * ch0) * (Math.Pow((ch1 / ch0), 1.4)));
            }
            else if (ratio <= 0.61)
            {
                luxVal = (float)(0.0224 * ch0) - (float)(0.031 * ch1);
            }
            else if (ratio <= 0.8)
            {
                luxVal = (float)(0.0128 * ch0) - (float)(0.0153 * ch1);
            }
            else if (ratio <= 1.3)
            {
                luxVal = (float)(0.00146 * ch0) - (float)(0.00112 * ch1);
            }

            Luminosity = luxVal;
        }
    }
}
