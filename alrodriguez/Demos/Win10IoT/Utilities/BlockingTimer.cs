using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
