using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoPinController.Utils
{
    public interface ITaskSchedulerUtility
    {
        TaskScheduler GetScheduler();
    }
}
