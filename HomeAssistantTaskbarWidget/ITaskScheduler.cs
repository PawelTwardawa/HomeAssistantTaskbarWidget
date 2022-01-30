using System;
using System.Threading.Tasks;

namespace HomeAssistantTaskbarWidget
{
    public interface ITaskScheduler
    {
        TaskScheduler SetInterval(int? seconds);
        TaskScheduler SetTaskHandler(Func<Task> task);
        TaskScheduler Start();
        TaskScheduler Stop();
    }
}
