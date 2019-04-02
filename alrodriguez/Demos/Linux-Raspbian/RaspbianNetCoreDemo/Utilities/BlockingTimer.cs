using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Utilities
{
    public class BlockingTimer : IBlockingTimer
    {
        private readonly int _delayTimeMilliseconds;

        public BlockingTimer(TimeSpan delayTime)
        {
            _delayTimeMilliseconds = (int)delayTime.TotalMilliseconds;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public void Run()
        {
            IsRunning = true;
            Thread.Sleep(_delayTimeMilliseconds);
            IsRunning = false;
        }
    }
}
