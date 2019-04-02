using System.Threading;
using System.Threading.Tasks;

namespace IoPinController.Utils
{
    public class TaskSchedulerUtility : ITaskSchedulerUtility
    {
        /// <summary>
        /// Gets the Scheduler to use for the given running context
        /// </summary>
        public TaskScheduler GetScheduler()
        {
            if (SynchronizationContext.Current != null)
            {
                return TaskScheduler.FromCurrentSynchronizationContext();
            }
            else
            {
                // If there is no SyncContext for this thread (e.g. we are in a unit test
                // or console scenario instead of running in an app), then just use the
                // default scheduler because there is no UI thread to sync with.
                return TaskScheduler.Current;
            }
        }
    }
}
