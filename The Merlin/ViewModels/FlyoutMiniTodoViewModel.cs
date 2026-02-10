using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;
using The_Merlin.Services;

namespace The_Merlin.ViewModels
{
    public class FlyoutMiniTodoViewModel : BaseViewModel
    {
        ITimerService _timerService;
        TodoData _todoData;

        public string dateString { get { return DateTime.Today.ToString("dd.MM.yy"); } }

        public FlyoutMiniTodoViewModel(ITimerService timerService, TodoData todoData)
        {
            _timerService = timerService;
            _todoData = todoData;
            timerService.Dispatcher().Tick += FlyoutMiniTodoViewModel_Tick;
        }

        private void FlyoutMiniTodoViewModel_Tick(object? sender, EventArgs e)
        {
            if (_timerService.ActiveTodoSession() != null)
            {
                TodoItem ats = _timerService.ActiveTodoSession();
                IsVisible = true;
                TodoTitle = ats.TodoText;
                IsManualCompletion = ats.CompletionType == TodoCompletionType.Manual;
                GradBackgroundColor = ats.Status == TodoItemStatus.InProgress ? Colors.DarkSlateGray : Colors.DarkOliveGreen;
                TodoTimer = _timerService.TimeString(ats);  
            }
            else
            {
                IsVisible = false;
            }
        }

        private Color _gradBackgroundColor = Colors.AliceBlue;
        public Color GradBackgroundColor { get { return _gradBackgroundColor; } set { _gradBackgroundColor = value; OnPropertyChanged(); } }

        private string _todoTitle;
        public string TodoTitle { get { return _todoTitle; } set { _todoTitle = value; OnPropertyChanged(); } }

        private string _todoTimer;
        public string TodoTimer { get { return _todoTimer; } set { _todoTimer = value; OnPropertyChanged(); } }

        private bool _isVisible;
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; OnPropertyChanged(); } }

        private bool _isManualCompletion;
        public bool IsManualCompletion { get { return _isManualCompletion; } set { _isManualCompletion = value; OnPropertyChanged("IsManualCompletion"); } }


        public ICommand GoToActiveCommand => new Command((Action)async delegate
        {
            var parameters = new Dictionary<string, object> { { "todo", await _todoData.GetItem(_timerService.ActiveTodoSession().Id) } };
            await Shell.Current.GoToAsync("TodoDetailView", parameters);
        });

        public ICommand StopCommand => new Command((Action)async delegate
        {
            await _timerService.StartStopTimer(_timerService.ActiveTodoSession());
        });

        public ICommand CompleteCommand => new Command((Action)async delegate
        {
            _timerService.ActiveTodoSession().Status = TodoItemStatus.Completed;
            await _timerService.StartStopTimer(_timerService.ActiveTodoSession());
        });

        public ICommand NavigateToDayList => new Command(async () =>
        {
            await Shell.Current.GoToAsync("DayListView");
        });

        public ICommand NavigateToTimelineLogsView => new Command(async () =>
        {
            await Shell.Current.GoToAsync("TimelineLogsView");
        });

        public ICommand NavigateToTodoDefListView => new Command(async () =>
        {
            await Shell.Current.GoToAsync("TodoDefListView");
        });
    }
}
