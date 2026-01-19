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
                _dataManager.TodoData.UpdateItem(value);
                TotalTimeString = _dataManager.TimelineData.GetTotal(value.Id).ToString(@"hh\:mm\:ss");
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
        private DataManager _dataManager;
            
        public TodoDetailViewModel(IMessageService msgService, ITimerService timerService, DataManager dataManager)
        {
            StartStopCommand = new Command(OnStartStop);
            DeleteCommand = new Command(DeleteAsync);
            _timerService = timerService;
            _messageService = msgService;
            _dataManager = dataManager;
            _timerService.Dispatcher().Tick += TodoDetailViewModel_Tick;

        }

        private void TodoDetailViewModel_Tick(object? sender, EventArgs e)
        {
            if (_timerService.IsTimerRunning())
                if (_timerService.ActiveTodoSession().Id == Todo.Id)
                    TimeString = _timerService.TimeString();
                else
                    TimeString = "Another todo running";
        }

        private void OnStartStop()
        {
            if (!_timerService.IsTimerRunning())
                if (_dataManager.TimelineData.AddItem(new TimelineItem { Starts = DateTime.Now, TodoId = Todo.Id }) == 1)
                {
                    _timerService.StartTimer(Todo);
                    TimerRunning = true;
                }
                else
                {
                    _messageService.ShowAsync("Problem", "Another todo running");
                }
            else
            {
                if (_timerService.ActiveTodoSession().Id == Todo.Id)
                {
                    _dataManager.TimelineData.EndItem(Todo.Id, DateTime.Now);
                    _timerService.StopTimer();
                    TimerRunning = false;
                }
                else
                {
                    _messageService.ShowAsync("Problem", "Another todo running - " + _timerService.ActiveTodoSession().TodoText);
                }
            }
        }

        private async void DeleteAsync()
        {
            _dataManager.TodoData.DeleteItem(Todo);
            await Shell.Current.Navigation.PopAsync();
        }
    }
}