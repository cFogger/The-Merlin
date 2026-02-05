using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.CustomControls;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<TimelineItem> TimelineIS { get; } = [];
        public ObservableCollection<TodoItem> todoItems { get; } = [];
        public List<TodoDefItem> TodoDefs;

        public string dateString { get { return DateTime.Today.ToString("dd.MM.yy"); } }

        private TimelineData _timelineData;
        private TodoData _todoData;
        private TodoDefData _todoDefData;
        private ITimerService _timer;

        public MainPageViewModel(TimelineData timelineData, TodoData todoData, TodoDefData todoDefData, ITimerService timerService)
        {
            _timelineData = timelineData;
            _todoData = todoData;
            _todoDefData = todoDefData;

            _timelineData.TimelineChanged += onTimelineChanged;
            _todoData.TodoItemCollectionChanged += onTodoItemsChanged;
            _todoDefData.TodoDefItemsChanged += onTodoDefsChanged;

            onTimelineChanged(this, EventArgs.Empty);
            onTodoItemsChanged(this, EventArgs.Empty);
            onTodoDefsChanged(this, EventArgs.Empty);

            DefSearchText = string.Empty;
        }

        public void onTimelineChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _timelineData.GetLastxItems(TimelineIS, 5);
            });
        }

        public void onTodoDefsChanged(object? sender, EventArgs e)
        {
            TodoDefs = _todoDefData.GetAllTodoDefItems();
        }

        public void onTodoItemsChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var items = _todoData.GetTodaysTodos();
                todoItems.Clear();
                foreach (var item in items)
                    todoItems.Add(item);
            });
        }

        public ICommand NavigateToTodoDetail => new Command<TodoItem>(async (ti) =>
        {
            var parameters = new Dictionary<string, object>
                {
                    { "todo", ti }
                };
            await Shell.Current.GoToAsync("TodoDetailView", parameters);
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

        public ICommand AddQuickTodoDefCommand => new Command(() =>
        {
            string temptext = DefSearchText;
            if (!string.IsNullOrEmpty(DefSearchText))
                _todoDefData.AddTodoDefItem(new TodoDefItem
                {
                    TodoDefText = DefSearchText,
                    RepeatType = TodoDefRepeatType.None,
                    DefaultCompletionType = TodoCompletionType.Manual,
                    DefaultDuration = TimeSpan.FromMinutes(25),
                });
            DefSearchText = string.Empty;
            DefSearchText = temptext;
        });

        public ICommand TodoAddCommand => new Command<TodoDefItem>((tdi) => { if (tdi == null) return; tdi.CreateTodoItem(_todoData); });

        public ObservableCollection<TodoDefItem> FilteredTodoDefs { get; } = [];

        private string _defSearchText;
        public string DefSearchText
        {
            get { return _defSearchText; }
            set
            {
                _defSearchText = value;
                FilteredTodoDefs.Clear();
                Debug.WriteLine("triggered via " + value);
                if (_defSearchText != string.Empty)
                    foreach (var item in TodoDefs.Where(x => x.TodoDefText.Contains(_defSearchText, StringComparison.OrdinalIgnoreCase)))
                        FilteredTodoDefs.Add(item);
                else
                    foreach (var item in TodoDefs)
                        FilteredTodoDefs.Add(item);
                OnPropertyChanged();
            }
        }
    }
}
