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
                _todo = value;
                OnPropertyChanged();
                TotalTimeString = _timelineData.GetTotalbyTodoId(value.Id).ToString(@"hh\:mm\:ss");
                TodoDefText = _todoDefData.GetTodoDefItemById(value.TodoDefId).TodoDefText;
                myTodoText = value.TodoText;
                IsManualCompletion = value.CompletionType == TodoCompletionType.Manual;
                TimerRunning = _timerService.IsTimerRunning();

                if (_timerService.ActiveTodoSession() != null)
                    IsStartVisible = _timerService.ActiveTodoSession()?.Id == value.Id;
                else
                    IsStartVisible = true;
            }
        }

        private bool _isManualCompletion;
        public bool IsManualCompletion { get { return _isManualCompletion; } set { _isManualCompletion = value; OnPropertyChanged(); } }

        private string _myTodoText;
        public string myTodoText
        {
            get { return _myTodoText; }
            set
            {
                _myTodoText = value;
                OnPropertyChanged();
                Todo.TodoText = value;
                _todoData.UpdateItem(Todo);
            }
        }

        private string _todoDefText;
        public string TodoDefText { get { return _todoDefText; } set { _todoDefText = value; OnPropertyChanged(); } }

        private string _totalTimeString;
        public string TotalTimeString { get { return _totalTimeString; } set { _totalTimeString = value; OnPropertyChanged(); } }

        private bool _isStartVisible;
        public bool IsStartVisible { get { return _isStartVisible; } set { _isStartVisible = value; OnPropertyChanged(); } }

        private bool _timerRunning;
        public bool TimerRunning { get { return _timerRunning; } set { _timerRunning = value; OnPropertyChanged(); } }

        private string _timeString;
        public string TimeString { get { return _timeString; } set { _timeString = value; OnPropertyChanged(); } }

        public ICommand DeleteCommand => new Command(async() => {
            _todoData.DeleteItem(Todo);
            await Shell.Current.Navigation.PopAsync();
        });

        public ICommand CompleteCommand => new Command((Action)async delegate
        {
            Todo.Status = TodoItemStatus.Completed;
            _todoData.UpdateItem(Todo);
            if (_timerService.ActiveTodoSession() != null && _timerService.ActiveTodoSession().Id == Todo.Id)
            {
                await _timerService.StartStopTimer(Todo);
            }
            else
            {
                string cntxt = await _messageService.ShowPromptAsync(Todo.TodoText, "N'aptın?", "Fill Context");
                _timelineData.AddANoTimeItem(new TimelineItem
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