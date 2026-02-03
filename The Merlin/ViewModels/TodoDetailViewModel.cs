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
        private TodoItem _todo;
        public TodoItem Todo
        {
            get { return _todo; }
            set
            {
                _todo = value;
                OnPropertyChanged();
                TotalTimeString = _timelineData.GetTotalbyTodoId(value.Id).ToString(@"hh\:mm\:ss");
                TodoDefText = _todoDefData.GetTodoDefItemById(Todo.TodoDefId).TodoDefText;
                myTodoText = Todo.TodoText;
                TimerRunning = false;
            }
        }

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


        public ICommand DeleteCommand => new Command(async() => {
            _todoData.DeleteItem(Todo);
            await Shell.Current.Navigation.PopAsync();
        });

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
        }

        //timer
        public ICommand StartStopCommand => new Command(async () =>
        {
            await _timerService.StartStopTimer(Todo);
            TimerRunning = true;
        });

        public void TodoDetailViewModel_Tick(object? sender, EventArgs e)
        {
            Debug.WriteLine("Tick TodoDetailViewModel" + Todo.TodoText);
            TimeString = _timerService.TimeString(Todo, TodoDetailViewModel_Tick);
        }

        private bool _timerRunning;
        public bool TimerRunning
        {
            get { return _timerRunning; }
            set
            {
                _timerRunning = _timerService.IsTimerRunning();
                OnPropertyChanged();
            }
        }

        private string _timeString;
        public string TimeString { get { return _timeString; } set { _timeString = value; OnPropertyChanged(); } }
    }
}