using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Utilities
{
    public interface IBlockingTimer
    {
        void Run();
        bool IsRunning { get; }
    }
}
