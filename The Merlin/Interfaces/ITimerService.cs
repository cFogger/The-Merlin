using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Interfaces
{
    public interface ITimerService
    {
        Task StartStopTimer(TodoItem tdi);
        IDispatcherTimer Dispatcher();
        string TimeString(TodoItem todo);
        bool IsTimerRunning();
        TodoItem ActiveTodoSession();
    }
}
