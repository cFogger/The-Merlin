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

        private TimelineData _timelineData;
        private readonly IMessageService _messageService;

        public IDispatcherTimer TodoTimer;

        public TimerService(TimelineData timelineData, TodoData todoData, IMessageService msgService)
        {
            _timelineData = timelineData;
            _messageService = msgService;
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
            if (IsChronoRunning) { 
                TodoTimeSpan = TodoTimeSpan.Add(TimeSpan.FromSeconds(1)); 
                TodoTimerText = TodoTimeSpan.ToString(@"hh\:mm\:ss"); }
        }

        public async Task StartStopTimer(TodoItem todo)
        {
            if (!IsChronoRunning)
            {
                if (_timelineData.AddItem(new TimelineItem { Starts = DateTime.Now, TodoId = todo.Id }) == 1)
                {
                    IsChronoRunning = true;
                    _ActiveTodoSession = todo;
                    TodoTimeSpan = TimeSpan.Zero;
                }
                else
                {
                    await _messageService.ShowAsync("Problem", "Another todo running - " + _ActiveTodoSession.TodoText);
                }
            }
            else
            {
                if (_ActiveTodoSession.Id == todo.Id)
                {
                    var cntxt = await _messageService.ShowPromptAsync(todo.TodoText, "N'aptın?", "Fill Context");
                    _timelineData.EndItem(todo.Id, DateTime.Now, cntxt);
                    IsChronoRunning = false;
                    _ActiveTodoSession = null;
                    TodoTimeSpan = TimeSpan.Zero;
                }
                else
                {
                    await _messageService.ShowAsync("Problem", "Another todo running - " + _ActiveTodoSession.TodoText);
                }
            }
        }

        public IDispatcherTimer Dispatcher()
        {
            return TodoTimer;
        }

        public string TimeString(TodoItem todo, Action<object?, EventArgs> evnt)
        {
            if (IsChronoRunning)
                if (_ActiveTodoSession.Id == todo.Id)
                {
                    return TodoTimerText;
                }
                else
                {
                    TodoTimer.Tick -= new EventHandler(evnt);
                    return "Another todo running";
                }
            else
            {
                TodoTimer.Tick -= new EventHandler(evnt);
                return "Timer is not running";
            }
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
