using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public interface IBlockingTimer
    {
        void Run();
        bool IsRunning { get; }
    }
}
