using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;

namespace The_Merlin.Services
{
    public class TimerService : ITimerService
    {
        public bool IsChronoRunning = false;
        public TimeSpan TodoTimeSpan;
        public TodoItem? _ActiveTodoSession = null;
        public string TodoTimerText;

        public IDispatcherTimer TodoTimer;

        public TimerService(TimelineData timelineData, TodoData todoData)
        {
            TimelineItem? tli = timelineData.checkRunningTodo();
            if (tli != null)
            {
                _ActiveTodoSession = todoData.GetItem(tli.TodoId);
                IsChronoRunning = true;
                TodoTimeSpan = DateTime.Now - tli.Starts;
            }

            TodoTimer = Application.Current.Dispatcher.CreateTimer();
            TodoTimer.Interval = TimeSpan.FromSeconds(1);
            TodoTimer.Start();
            TodoTimer.Tick += TodoTimer_Tick;
        }

        private void TodoTimer_Tick(object? sender, EventArgs e)
        {
            if (IsChronoRunning) { TodoTimeSpan = TodoTimeSpan.Add(TimeSpan.FromSeconds(1)); TodoTimerText = TodoTimeSpan.ToString(@"hh\:mm\:ss"); }
        }

        public Task StartTimer(TodoItem todo)
        {
            IsChronoRunning = true;
            _ActiveTodoSession = todo;
            TodoTimeSpan = TimeSpan.Zero;
            return Task.CompletedTask;
        }

        public Task StopTimer()
        {
            IsChronoRunning = false;
            _ActiveTodoSession = null;
            TodoTimeSpan = TimeSpan.Zero;
            return Task.CompletedTask;
        }

        public IDispatcherTimer Dispatcher()
        {
            return TodoTimer;
        }

        public string TimeString()
        {
            return TodoTimerText;
        }

        public bool IsTimerRunning()
        {
            return IsChronoRunning;
        }

        public TodoItem ActiveTodoSession()
        {
            return _ActiveTodoSession;
        }
    }
}
