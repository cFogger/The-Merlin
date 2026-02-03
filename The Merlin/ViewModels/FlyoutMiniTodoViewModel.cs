using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using The_Merlin.Interfaces;

namespace The_Merlin.ViewModels
{
    public class FlyoutMiniTodoViewModel : BaseViewModel
    {
        ITimerService TimerService;

        private string _todoTitle;
        public string TodoTitle { get { return _todoTitle; } set { _todoTitle = value; OnPropertyChanged(); } }

        private string _todoTimer;
        public string TodoTimer { get { return _todoTimer; } set { _todoTimer = value; OnPropertyChanged(); } }

        private bool _isVisible;
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; OnPropertyChanged(); } }

        public Command GoToActive { get; }

        public FlyoutMiniTodoViewModel(ITimerService timerService)
        {
            TimerService = timerService;
            timerService.Dispatcher().Tick += FlyoutMiniTodoViewModel_Tick;
            GoToActive = new Command(_gotoActive);
        }

        private void FlyoutMiniTodoViewModel_Tick(object? sender, EventArgs e)
        {
            if (TimerService.ActiveTodoSession() != null)
            {
                IsVisible = true;
                TodoTitle = TimerService.ActiveTodoSession().TodoText;
                TodoTimer = TimerService.TimeString(TimerService.ActiveTodoSession(), FlyoutMiniTodoViewModel_Tick);
            }
            else
            {
                IsVisible = false;
            }
        }

        private async void _gotoActive()
        {
            var parameters = new Dictionary<string, object>
            {
                {"todo",TimerService.ActiveTodoSession()},
            };
            await Shell.Current.GoToAsync("TodoDetailView", parameters);
        }
    }
}
