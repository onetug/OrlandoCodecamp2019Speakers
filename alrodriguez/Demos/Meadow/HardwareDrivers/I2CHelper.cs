using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Meadow.Hardware.I2cPeripheral;

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

        private readonly I2CWriteTransaction _writeTransaction;
        private readonly I2CWriteTransaction _writeUshortTransaction;
        private readonly I2CReadTransaction _readTransaction;

        private readonly I2CTransaction[] _readTransactions;
        private readonly I2CTransaction[] _writeTransactions;
        private readonly I2CTransaction[] _writeUshortTransactions;

        private readonly I2cPeripheral _device;

        public I2CHelper(I2cPeripheral device)
        {
            _device = device;
            _readTransaction = I2cPeripheral.CreateReadTransaction(_readWriteIndividualValueBuffer);
            _writeTransaction = I2cPeripheral.CreateWriteTransaction(_addressWriteIndividualValueBuffer);
            _writeUshortTransaction = I2cPeripheral.CreateWriteTransaction(_ushortWriteValueBuffer);

            _readTransactions = new[] { _readTransaction };
            _writeTransactions = new[] { _writeTransaction };
            _writeUshortTransactions = new[] { _writeUshortTransaction };
        }

        public void WriteByteToRegister(byte registerAddress, byte value)
        {
            _addressWriteIndividualValueBuffer[0] = registerAddress;
            _addressWriteIndividualValueBuffer[1] = value;

            _device.Execute(_writeTransactions, 1000);
        }

        public byte ReadByteFromRegister(byte registerAddress)
        {
            _readWriteIndividualValueBuffer[0] = registerAddress;

            _device.Execute(_readTransactions, 1000);
            return _readWriteIndividualValueBuffer[0];
        }

        public byte[] ReadRegisters(byte registerAddress, ushort length)
        {
            _readWriteIndividualValueBuffer[0] = registerAddress;
            byte[] registerReadBuffer = new byte[length];
            var registerReadTransaction = I2cPeripheral.CreateReadTransaction(registerReadBuffer);

            var readRegistersTransactions = new[] { _readTransaction, registerReadTransaction };
            _device.Execute(readRegistersTransactions, 1000);

            return registerReadBuffer;
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
            _device.Execute(_writeUshortTransactions, 1000);
        }
    }
}
