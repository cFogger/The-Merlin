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
        private TodoData _todoData;

        private bool _isAlerted = false;
        private TimeSpan _lastTotal = TimeSpan.Zero;

        public IDispatcherTimer TodoTimer = App.AppTimer;

        public TimerService(TimelineData timelineData, TodoData todoData, IMessageService msgService)
        {
            _timelineData = timelineData;
            _todoData = todoData;
            _messageService = msgService;

            TimelineItem? tli = timelineData.checkRunningTodo();
            if (tli != null)
            {
                _ActiveTodoSession = todoData.GetItem(tli.TodoId);
                IsChronoRunning = true;
                TodoTimeSpan = DateTime.Now - tli.Starts;
                TimerStarted?.Invoke(this, new EventArgs());
            }

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
                if (_timelineData.AddItem(new TimelineItem
                {
                    Starts = DateTime.Now,
                    TodoId = todo.Id
                }) == 1)
                {
                    _isAlerted = false;
                    IsChronoRunning = true;
                    _ActiveTodoSession = todo;
                    TodoTimeSpan = TimeSpan.Zero;
                    _lastTotal = _timelineData.GetTotalbyTodoId(todo.Id);
                    if (todo.Status == TodoItemStatus.Pending)
                    {
                        todo.Status = TodoItemStatus.InProgress;
                    }
                    _todoData.UpdateItem(todo);
                    TimerStarted?.Invoke(this, new EventArgs());
                }
                else
                {
                    await _messageService.ShowAsync("Problem", "Another todo running - " + _ActiveTodoSession.TodoText);
                }
            }
            else if (_ActiveTodoSession.Id == todo.Id)
            {
                string cntxt = await _messageService.ShowPromptAsync(todo.TodoText, "N'aptın?", "Fill Context");
                _timelineData.EndItem(todo.Id, DateTime.Now, cntxt);
                if (todo.CompletionType == TodoCompletionType.DurationBased)
                {
                    if (todo.Duration > _lastTotal.Add(TodoTimeSpan))
                    {
                        todo.Status = TodoItemStatus.Pending;
                    }
                    else
                    {
                        todo.Status = TodoItemStatus.Completed;
                    }
                }
                _todoData.UpdateItem(todo);
                IsChronoRunning = false;
                _ActiveTodoSession = null;
                TodoTimeSpan = TimeSpan.Zero; 
                TimerStopped?.Invoke(this, new EventArgs());
            }
            else
            {
                await _messageService.ShowAsync("Problem", "Another todo running - " + _ActiveTodoSession.TodoText);
            }
        }

        public IDispatcherTimer Dispatcher()
        {
            return TodoTimer;
        }

        public string TimeString(TodoItem todo)
        {
            if (IsChronoRunning)
            {
                if (_ActiveTodoSession.Id == todo.Id)
                {
                    if (!_isAlerted && todo.CompletionType == TodoCompletionType.DurationBased && todo.Duration < _lastTotal.Add(TodoTimeSpan))
                    {
                        HandleCompletionAlert(todo);
                    }
                    return TodoTimerText;
                }
                return TodoTimerText + " " + _ActiveTodoSession.TodoText;
            }
            return "00:00:00";
        }

        private async void HandleCompletionAlert(TodoItem todo)
        {
            _isAlerted = true;
            if (await _messageService.ShowConfirmAsync("Todo Time Exceeded", $"The time spent on {todo.TodoText} has exceeded the expected duration of {todo.Duration}. Do you want to continue?"))
            {
                todo.Status = TodoItemStatus.Completed;
                _todoData.UpdateItem(todo);
            }
            else
            {
                await StartStopTimer(todo);
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

        public event EventHandler TimerStopped;
        public event EventHandler TimerStarted;
    }
}
