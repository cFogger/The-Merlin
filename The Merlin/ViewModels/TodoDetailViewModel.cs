using The_Merlin.Interfaces;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(Todo), "todo")]
    public class TodoDetailViewModel : BaseViewModel
    {
        private TodoItem _todo;
        public TodoItem Todo { get { return _todo; } set { _todo = value; OnPropertyChanged(); App.DataManager.TodoData.UpdateItem(value); } }

        private string _timeString;
        public string TimeString { get { return _timeString; } set { _timeString = value; OnPropertyChanged(); } }

        public Command StartStopCommand { get; }
        public Command DeleteCommand { get; }

        private IMessageService _messageService;
        private ITimerService _timerService;

        public TodoDetailViewModel(IMessageService msgService, ITimerService timerService)
        {
            StartStopCommand = new Command(OnStartStop);
            DeleteCommand = new Command(DeleteAsync);
            _timerService = timerService;
            _messageService = msgService;
            _timerService.Dispatcher().Tick += TodoDetailViewModel_Tick;
        }

        private void TodoDetailViewModel_Tick(object? sender, EventArgs e)
        {
            TimeString = _timerService.TimeString();
        }

        private void OnStartStop()
        {
            if (!_timerService.IsTimerRunning())
                if (App.DataManager.TimelineData.AddItem(new TimelineItem { Starts = DateTime.Now, TodoId = Todo.Id }) == 1)
                {
                    _timerService.StartTimer(Todo);
                }
                else
                {
                    _messageService.ShowAsync("Problem", "Another todo running");
                }
            else
            {
                if (_timerService.ActiveTodoSession().Id == Todo.Id)
                {
                    App.DataManager.TimelineData.EndItem(Todo.Id, DateTime.Now);
                    _timerService.StopTimer();
                }
                else
                {
                    _messageService.ShowAsync("Problem", "Another todo running - " + _timerService.ActiveTodoSession().TodoText);
                }
            }
        }

        private async void DeleteAsync()
        {
            App.DataManager.TodoData.DeleteItem(Todo);
            await Shell.Current.Navigation.PopAsync();
        }
    }
}