using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Interfaces
{
    public interface ITimerService
    {
        Task StartTimer(TodoItem tdi);
        Task StopTimer();
        IDispatcherTimer Dispatcher();
        string TimeString();
        bool IsTimerRunning();
        TodoItem ActiveTodoSession();
    }
}
