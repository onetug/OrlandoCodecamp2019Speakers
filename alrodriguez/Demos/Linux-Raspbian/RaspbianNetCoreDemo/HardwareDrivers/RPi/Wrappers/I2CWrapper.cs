using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HardwareDrivers.RPi.Wrappers
{
    public static class I2CWrapper
    {
        [DllImport("./NativeLibs/i2cNativeLib.o", EntryPoint = "openBus", SetLastError = true)]
        public static extern int OpenBus(string busFileName);

        [DllImport("./NativeLibs/i2cNativeLib.o", EntryPoint = "closeBus", SetLastError = true)]
        public static extern int CloseBus(int busHandle);

        [DllImport("./NativeLibs/i2cNativeLib.o", EntryPoint = "readBytes", SetLastError = true)]
        public static extern int ReadBytes(int busHandle, int addr, byte[] buf, int len);

        [DllImport("./NativeLibs/i2cNativeLib.o", EntryPoint = "writeBytes", SetLastError = true)]
        public static extern int WriteBytes(int busHandle, int addr, byte[] buf, int len);
    }
}
