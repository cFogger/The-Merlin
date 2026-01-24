using System.Threading.Tasks;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(Todo), "todo")]
    public class TodoDetailViewModel : BaseViewModel
    {
        private TodoItem _todo;
        public TodoItem Todo
        {
            get { return _todo; }
            set
            {
                _todo = value;
                OnPropertyChanged();
                _todoData.UpdateItem(value);
                TotalTimeString = _timelineData.GetTotal(value.Id).ToString(@"hh\:mm\:ss");
                if (_timerService.IsTimerRunning() && _timerService.ActiveTodoSession().Id == value.Id)
                    TimerRunning = true;
                else
                    TimerRunning = false;
            }
        }

        private string _timeString;
        public string TimeString { get { return _timeString; } set { _timeString = value; OnPropertyChanged(); } }

        private string _totalTimeString;
        public string TotalTimeString { get { return _totalTimeString; } set { _totalTimeString = value; OnPropertyChanged(); } }

        private string _startButtonText;
        public string StartButtonText
        {
            get { return _startButtonText; }
            set { _startButtonText = value; OnPropertyChanged(); }
        }

        private bool _timerRunning;
        public bool TimerRunning
        {
            get { return _timerRunning; }
            set
            {
                _timerRunning = value;
                OnPropertyChanged();

                if (value)
                    StartButtonText = "Stop";
                else
                    StartButtonText = "Start";
            }
        }

        public Command StartStopCommand { get; }
        public Command DeleteCommand { get; }

        private IMessageService _messageService;
        private ITimerService _timerService;
        private TimelineData _timelineData;
        private TodoData _todoData;

        public TodoDetailViewModel(IMessageService msgService, ITimerService timerService, TimelineData timelineData, TodoData todoData)
        {
            StartStopCommand = new Command(OnStartStop);
            DeleteCommand = new Command(DeleteAsync);
            _timerService = timerService;
            _messageService = msgService;
            _timerService.Dispatcher().Tick += TodoDetailViewModel_Tick;
            _timelineData = timelineData;
            _todoData = todoData;
        }

        private void TodoDetailViewModel_Tick(object? sender, EventArgs e)
        {
            if (_timerService.IsTimerRunning())
                if (_timerService.ActiveTodoSession().Id == Todo.Id)
                    TimeString = _timerService.TimeString();
                else
                    TimeString = "Another todo running";
        }

        private async void OnStartStop()
        {
            if (!_timerService.IsTimerRunning())
                if (_timelineData.AddItem(new TimelineItem { Starts = DateTime.Now, TodoId = Todo.Id }) == 1)
                {
                    await _timerService.StartTimer(Todo);
                    TimerRunning = true;
                }
                else
                {
                    await _messageService.ShowAsync("Problem", "Another todo running");
                }
            else
            {
                if (_timerService.ActiveTodoSession().Id == Todo.Id)
                {
                    await EndTimelineContextAsync();
                    await _timerService.StopTimer();
                    TimerRunning = false;
                }
                else
                {
                    await _messageService.ShowAsync("Problem", "Another todo running - " + _timerService.ActiveTodoSession().TodoText);
                }
            }
        }

        private async Task EndTimelineContextAsync()
        {
            var cntxt = await _messageService.ShowPromptAsync(Todo.TodoText, "N'aptın?", "Fill Context");
            _timelineData.EndItem(Todo.Id, DateTime.Now, cntxt);
        }

        private async void DeleteAsync()
        {
            _todoData.DeleteItem(Todo);
            await Shell.Current.Navigation.PopAsync();
        }
    }
}