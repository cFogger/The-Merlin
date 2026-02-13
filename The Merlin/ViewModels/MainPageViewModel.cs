using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.CustomControls;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;
using The_Merlin.Services;

namespace The_Merlin.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<TimelineItem> TimelineLast5 { get; } = [];
        public ObservableCollection<TimelineItem> TodaysTimelineItems { get; } = [];
        public ObservableCollection<TodoItem> todoItems { get; } = [];
        public List<TodoDefItem> TodoDefs;

        public string dateString { get { return DateTime.Today.ToString("dd.MM.yy"); } }
        public IDispatcherTimer myDispatcher { get { return _timer.Dispatcher(); } }

        private TimelineData _timelineData;
        private TodoData _todoData;
        private TodoDefData _todoDefData;
        private DayData _dayData;
        private ITimerService _timer;

        public MainPageViewModel(TimelineData timelineData, TodoData todoData, TodoDefData todoDefData, DayData dayData, ITimerService timer)
        {
            _timelineData = timelineData;
            _todoData = todoData;
            _todoDefData = todoDefData;
            _dayData = dayData;
            _timer = timer;
        }

        public async void Load()
        {
            _timelineData.TimelineChanged += onTimelineChanged;
            _todoData.TodoItemCollectionChanged += onTodoItemsChanged;
            _todoDefData.TodoDefItemsChanged += onTodoDefsChanged;

            onTimelineChanged(this, EventArgs.Empty);
            onTodoItemsChanged(this, EventArgs.Empty);
            onTodoDefsChanged(this, EventArgs.Empty);

            MyDayItem = await _dayData.GetToday();
            DayDesc = MyDayItem.Content;
        }

        public async void Unload()
        {
            _timelineData.TimelineChanged -= onTimelineChanged;
            _todoData.TodoItemCollectionChanged -= onTodoItemsChanged;
            _todoDefData.TodoDefItemsChanged -= onTodoDefsChanged;
        }

        public void onTimelineChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _timelineData.GetLastxItems(TimelineLast5, 5);
                await _timelineData.GetItemsByDate(TodaysTimelineItems);
            });
        }

        public async void onTodoDefsChanged(object? sender, EventArgs e)
        {
            TodoDefs = await _todoDefData.GetAllTodoDefItems();
            DefSearchText = string.Empty;
        }

        public void onTodoItemsChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _todoData.GetTodos(todoItems, DateTime.Today);
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

        public ICommand AddQuickTodoDefCommand => new Command(async () =>
        {
            string temptext = DefSearchText;
            if (!string.IsNullOrEmpty(DefSearchText))
                await _todoDefData.AddTodoDefItem(new TodoDefItem
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
                if (_defSearchText != string.Empty)
                    foreach (var item in TodoDefs.Where(x => x.TodoDefText.Contains(_defSearchText, StringComparison.OrdinalIgnoreCase)))
                        FilteredTodoDefs.Add(item);
                else
                    foreach (var item in TodoDefs)
                        FilteredTodoDefs.Add(item);

                OnPropertyChanged();
            }
        }

        private DayItem _myDayItem;
        public DayItem MyDayItem { get { return _myDayItem; } set { _myDayItem = value; OnPropertyChanged(); } }

        //debounce
        CancellationTokenSource cts;
        private string _dayDesc;
        public string DayDesc
        {
            get { return _dayDesc; }
            set
            {
                _dayDesc = value;

                cts?.Cancel();
                cts = new CancellationTokenSource();
                MyDayItem.Content = _dayDesc;
                OnPropertyChanged();
                Task.Delay(1000, cts.Token).ContinueWith(async t =>
                {
                    if (!t.IsCanceled)
                        await _dayData.UpdateItem(MyDayItem);
                });
            }
        }
    }
}
