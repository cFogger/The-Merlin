using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(Todo), "todo")]
    public class TodoDetailViewModel : BaseViewModel
    {
        private IMessageService _messageService;
        public ITimerService _timerService;
        private TimelineData _timelineData;
        private TodoData _todoData;
        private TodoDefData _todoDefData;

        public TodoDetailViewModel(IMessageService msgService, ITimerService timerService, TimelineData timelineData, TodoData todoData, TodoDefData todoDefData)
        {
            _timerService = timerService;
            _messageService = msgService;
            _timelineData = timelineData;
            _todoData = todoData;
            _todoDefData = todoDefData;

            _timerService.TimerStarted += _timerService_TimerStarted;
            _timerService.TimerStopped += _timerService_TimerStopped;
        }

        private void _timerService_TimerStopped(object? sender, EventArgs e)
        {
            TimerRunning = false;
            if (_timerService.ActiveTodoSession() != null)
                IsStartVisible = _timerService.ActiveTodoSession()?.Id == Todo.Id;
            else
                IsStartVisible = true;
        }

        private void _timerService_TimerStarted(object? sender, EventArgs e)
        {
            TimerRunning = true;
        }


        private TodoItem _todo;
        public TodoItem Todo
        {
            get { return _todo; }
            set
            {
                if (_todo == value) return;

                _todo = value;
                OnPropertyChanged();
                myTodoText = value.TodoText;
                IsManualCompletion = value.CompletionType == TodoCompletionType.Manual;
                TimerRunning = _timerService.IsTimerRunning();
                Load();
                if (_timerService.ActiveTodoSession() != null)
                    IsStartVisible = _timerService.ActiveTodoSession()?.Id == value.Id;
                else
                    IsStartVisible = true;
            }
        }

        private async void Load()
        {
            var value1 = await _timelineData.GetTotalbyTodoId(_todo.Id);
            TotalTimeString = value1.ToString(@"hh\:mm\:ss");

            StartTimeSelected = EndTimeSelected = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        private bool _isManualCompletion;
        public bool IsManualCompletion { get { return _isManualCompletion; } set { _isManualCompletion = value; OnPropertyChanged(); } }


        CancellationTokenSource cts;
        private string _myTodoText;
        public string myTodoText
        {
            get { return _myTodoText; }
            set
            {
                if (_myTodoText == value) return;
                _myTodoText = value;
                OnPropertyChanged();
                cts?.Cancel();
                cts = new CancellationTokenSource();
                Todo.TodoText = value;
                Task.Delay(1000, cts.Token).ContinueWith(async t =>
                {
                    if (!t.IsCanceled)
                        await _todoData.SaveItem(Todo);
                });
            }
        }

        private string _totalTimeString;
        public string TotalTimeString { get { return _totalTimeString; } set { _totalTimeString = value; OnPropertyChanged(); } }

        private bool _isStartVisible;
        public bool IsStartVisible { get { return _isStartVisible; } set { _isStartVisible = value; OnPropertyChanged(); } }

        private bool _timerRunning;
        public bool TimerRunning { get { return _timerRunning; } set { _timerRunning = value; OnPropertyChanged(); } }

        private string _timeString;
        public string TimeString { get { return _timeString; } set { _timeString = value; OnPropertyChanged(); } }

        private TimeSpan _startTimeSelected;
        private TimeSpan _endTimeSelected;
        public TimeSpan StartTimeSelected { get { return _startTimeSelected; } set { _startTimeSelected = value; OnPropertyChanged(); } }
        public TimeSpan EndTimeSelected { get { return _endTimeSelected; } set { _endTimeSelected = value; OnPropertyChanged(); } }

        public ICommand AddManualLogCommand => new Command(async () =>
        {
            string cntxt = await _messageService.ShowPromptAsync(Todo.TodoText, "N'aptın?", "Fill Context");
            await _timelineData.SaveItem(new TimelineItem
            {
                TodoId = Todo.Id,
                Starts = Todo.AssignedDate.Date + StartTimeSelected,
                Ends = Todo.AssignedDate.Date + EndTimeSelected,
                Context = cntxt
            });
        });

        public ICommand DeleteCommand => new Command(async() => {
            await _todoData.DeleteItem(Todo.Id);
            await Shell.Current.Navigation.PopAsync();
        });

        public ICommand CompleteCommand => new Command((Action)async delegate
        {
            Todo.Status = TodoItemStatus.Completed;
            await _todoData.SaveItem(Todo);
            if (_timerService.ActiveTodoSession() != null && _timerService.ActiveTodoSession().Id == Todo.Id)
            {
                await _timerService.StartStopTimer(Todo);
            }
            else
            {
                string cntxt = await _messageService.ShowPromptAsync(Todo.TodoText, "N'aptın?", "Fill Context");
                await _timelineData.SaveItem(new TimelineItem
                {
                    TodoId = Todo.Id,
                    Starts = DateTime.Now.AddSeconds(-1.0),
                    Ends = DateTime.Now,
                    Context = cntxt
                });
            }
        });

        public ICommand StartStopCommand => new Command(async () =>
        {
            await _timerService.StartStopTimer(Todo);
        });
    }
}