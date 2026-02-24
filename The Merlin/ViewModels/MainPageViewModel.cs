using CommunityToolkit.Maui.Extensions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.CustomControls;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;
using The_Merlin.Services;
using System.Linq;

namespace The_Merlin.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<TimelineItem> TodaysTimelineItems { get; } = [];
        public ObservableCollection<TodoItem> todoItems { get; } = [];
        public ObservableCollection<TodoDefItem> TodoDefs { get; } = [];
        public ObservableCollection<TodoDefItem> FilteredTodoDefs { get; } = [];

        public ObservableCollection<HabitItem> HabitDefs { get; } = [];
        public ObservableCollection<HabitItem> FilteredHabitDefs { get; } = [];
        public ObservableCollection<HabitItem> TodaysHabits { get; } = []; // DayItem JSON'dan dolacak
        public ObservableCollection<HabitHistoryItem> TodaysHabitHistory { get; } = [];

        public string dateString { get { return DateTime.Today.ToString("dd.MM.yy"); } }
        public IDispatcherTimer myDispatcher { get { return _timer.Dispatcher(); } }

        private TimelineData _timelineData;
        private TodoData _todoData;
        private TodoDefData _todoDefData;
        private DayData _dayData;
        private HabitData _habitData;
        private HabitHistoryData _habitHistoryData;
        private ITimerService _timer;

        bool isFirstLoad = true;

        public MainPageViewModel(TimelineData timelineData, TodoData todoData, TodoDefData todoDefData, DayData dayData, HabitData habitData, HabitHistoryData habitHistoryData, ITimerService timer)
        {
            _timelineData = timelineData;
            _todoData = todoData;
            _todoDefData = todoDefData;
            _dayData = dayData;
            _habitData = habitData;
            _habitHistoryData = habitHistoryData;
            _timer = timer;
        }

        public async void Load()
        {
            if (isFirstLoad)
            {
                _timelineData.TimelineChanged += onTimelineChanged;
                _todoData.TodoItemCollectionChanged += onTodoItemsChanged;
                _todoDefData.TodoDefItemsChanged += onTodoDefsChanged;
                _habitData.HabitItemsChanged += onHabitItemsChanged;
                _habitHistoryData.HabitHistoryItemsChanged += onHabitHistoryChanged;
                _timer.TimerStarted += _timer_TimerStarted;
                _timer.TimerStopped += _timer_TimerStarted;

                onTimelineChanged(this, EventArgs.Empty);
                onTodoItemsChanged(this, EventArgs.Empty);
                onTodoDefsChanged(this, EventArgs.Empty);
                onHabitItemsChanged(this, EventArgs.Empty);
                onHabitHistoryChanged(this, EventArgs.Empty);
                _timer_TimerStarted(this, EventArgs.Empty);

                isFirstLoad = false;
                MyDayItem = await _dayData.GetToday();
            }
            onTodaysHabitChanged();
            Debug.WriteLine(MyDayItem.Habits.Count());
            DayDesc = MyDayItem.Content;
        }

        private void _timer_TimerStarted(object? sender, EventArgs e)
        {           
            IsTimerAvailable = true;
        }

        public void onTodaysHabitChanged()
        {
            TodaysHabits.Clear();
            foreach (var habit in MyDayItem.Habits)
            {
                TodaysHabits.Add(habit);
            }
        }

        public void onTimelineChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _timelineData.GetItemsByDate(TodaysTimelineItems);
            });
        }

        public void onHabitHistoryChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _habitHistoryData.GetHabitHistoryItems(TodaysHabitHistory);
            });
        }

        public async void onTodoDefsChanged(object? sender, EventArgs e)
        {
            await _todoDefData.GetTodoDefItems(TodoDefs);
            DefSearchText = string.Empty;
        }

        public async void onHabitItemsChanged(object? sender, EventArgs e)
        {
            await _habitData.GetHabitItems(HabitDefs);
            HabitSearchText = string.Empty; // Arama metnini sıfırla
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
            //var parameters = new Dictionary<string, object>
            //    {
            //        { "todo", ti }
            //    };
            //await Shell.Current.GoToAsync("TodoDetailView", parameters);

            var popupresult = await App.Current.Windows[0].Page.ShowPopupAsync<TodoItem>(new TodoItemDetailPopup(ti));
            if (!popupresult.WasDismissedByTappingOutsideOfPopup && popupresult.Result != null)
            {
                await _todoData.SaveItem(popupresult.Result);
                ti = popupresult.Result;
            }
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
                    DefaultDuration = TimeSpan.FromMinutes(25)
                });
            DefSearchText = string.Empty;
            DefSearchText = temptext;
        });

        public ICommand AddQuickHabitCommand => new Command(async () =>
        {
            string temptext = HabitSearchText;
            if (!string.IsNullOrEmpty(HabitSearchText))
                await _habitData.AddHabitItem(new HabitItem
                {
                    Title = HabitSearchText,
                    CreatedAt = DateTime.Now,
                    TotalCount = 0,
                    DailyMaxCount = 0,
                    DailyMinCount = 0,
                    IsPositive = true,
                });
            HabitSearchText = string.Empty;
            HabitSearchText = temptext;
        });

        public ICommand TodoAddCommand => new Command<TodoDefItem>((tdi) => { if (tdi == null) return; tdi.CreateTodoItem(_todoData); });

        public ICommand AddHabitToDayCommand => new Command<HabitItem>(async (hi) =>
        {
            if (hi == null) return;
            if (MyDayItem.Habits.Any(h => h.Id == hi.Id)) return; // Aynı alışkanlık zaten eklenmişse ekleme
            if (MyDayItem.Habits.Count == 0) MyDayItem.Habits = new List<HabitItem>() { hi };
            else MyDayItem.Habits.Add(hi);

            await _dayData.UpdateItem(MyDayItem);
            onTodaysHabitChanged();
        });

        public ICommand IncrementHabitCommand =>
            new Command<HabitItem>((hi) =>
            {
                hi.TotalCount++;
                hi.AddHabitHistory(_habitHistoryData, 1);
            });

        private bool _isTimerAvailable;
        public bool IsTimerAvailable
        {
            get { return _isTimerAvailable; }
            set { _isTimerAvailable = !_timer.IsTimerRunning(); OnPropertyChanged(); }
        }

        public ICommand StartTimerCommand => new Command<TodoItem>(async (ti) => { if (ti == null) return; if (IsTimerAvailable) await _timer.StartStopTimer(ti); });

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

        private string _habitSearchText;
        public string HabitSearchText
        {
            get { return _habitSearchText; }
            set
            {
                _habitSearchText = value;
                FilteredHabitDefs.Clear();
                if (!string.IsNullOrEmpty(_habitSearchText))
                {
                    foreach (var item in HabitDefs.Where(x => x.Title.Contains(_habitSearchText, StringComparison.OrdinalIgnoreCase)))
                    {
                        FilteredHabitDefs.Add(item);
                    }
                }
                else
                {
                    foreach (var item in HabitDefs)
                    {
                        FilteredHabitDefs.Add(item);
                    }
                }
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
                if (_dayDesc == value) return;

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
