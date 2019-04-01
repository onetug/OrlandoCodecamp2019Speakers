using HardwareDrivers.RPi;
using System;

namespace HardwareDrivers
{
    public enum ByteOrder : byte
    {
        LittleEndian,
        BigEndian
    }

    public class I2CHelper
    {
        /// <summary>
        /// This bit control the write operations.
        /// Setting this bit puts the chip into Word mode for the specified register.
        /// </summary>
        private const byte WordModeBit = 0x20;

        private readonly byte[] _readWriteIndividualValueBuffer = new byte[1];
        private readonly byte[] _addressWriteIndividualValueBuffer = new byte[2];
        private readonly byte[] _ushortWriteValueBuffer = new byte[3];

        private readonly I2cDevice _device;

        public I2CHelper(I2cDevice device)
        {
            _device = device;
        }

        public void WriteByteToRegister(byte registerAddress, byte value)
        {
            _addressWriteIndividualValueBuffer[0] = registerAddress;
            _addressWriteIndividualValueBuffer[1] = value;

            _device.Write(_addressWriteIndividualValueBuffer);
        }

        public byte ReadByteFromRegister(byte registerAddress)
        {
            _readWriteIndividualValueBuffer[0] = registerAddress;
            _device.WriteRead(_readWriteIndividualValueBuffer, _readWriteIndividualValueBuffer);
            return _readWriteIndividualValueBuffer[0];
        }

        public byte[] ReadRegisters(byte registerAddress, ushort length)
        {
            _readWriteIndividualValueBuffer[0] = registerAddress;
            byte[] readBuffer = new byte[length];
            _device.WriteRead(_readWriteIndividualValueBuffer, readBuffer);

            return readBuffer;
        }

        public ushort[] ReadUShorts(byte registerAddress, ushort number, ByteOrder order)
        {
            byte[] data = ReadRegisters(registerAddress, (ushort)((2 * number) & 0xffff));
            ushort[] result = new ushort[number];
            for (int index = 0; index < number; index++)
            {
                if (order == ByteOrder.LittleEndian)
                {
                    result[index] = (ushort)((data[(2 * index) + 1] << 8) + data[2 * index]);
                }
                else
                {
                    result[index] = (ushort)((data[2 * index] << 8) + data[(2 * index) + 1]);
                }
            }
            return result;
        }

        public ushort ReadUShort(byte registerAddress, ByteOrder order)
        {
            byte wordAddress = (byte)(WordModeBit | registerAddress);
            byte[] data = ReadRegisters(wordAddress, 2);
            ushort result = 0;
            if (order == ByteOrder.LittleEndian)
            {
                result = (ushort)((data[1] << 8) + data[0]);
            }
            else
            {
                result = (ushort)((data[0] << 8) + data[1]);
            }
            return result;
        }

        public void WriteUShort(byte registerAddress, ushort value, ByteOrder order)
        {
            byte wordAddress = (byte)(WordModeBit | registerAddress);
            _ushortWriteValueBuffer[0] = wordAddress;
            if (order == ByteOrder.LittleEndian)
            {
                _ushortWriteValueBuffer[1] = (byte)(value & 0xff);
                _ushortWriteValueBuffer[2] = (byte)((value >> 8) & 0xff);
            }
            else
            {
                _ushortWriteValueBuffer[1] = (byte)((value >> 8) & 0xff);
                _ushortWriteValueBuffer[2] = (byte)(value & 0xff);
            }
            _device.Write(_ushortWriteValueBuffer);
        }
    }
}
