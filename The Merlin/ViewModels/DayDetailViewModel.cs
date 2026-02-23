using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(myDayItem), "dayitem")]
    public class DayDetailViewModel : BaseViewModel
    {
        public DayItem myDayItem { get { return _myDayItem; } set { 
                _myDayItem = value; 
                OnPropertyChanged();

                onTodoDefsChanged(this, EventArgs.Empty);
                onTodoItemsChanged(this, EventArgs.Empty);
                loadTimelineItems();
            }
        }
        private DayItem _myDayItem;

        public ObservableCollection<TodoItem> TodoItems { get; } = [];
        public ObservableCollection<TodoDefItem> TodoDefs { get; } = [];
        public ObservableCollection<TimelineItem> TimelineItems { get; } = [];
        public ObservableCollection<TodoDefItem> FilteredTodoDefs { get; } = [];
        public ObservableCollection<StatItem> StatItems { get; } = [];

        private TodoData _todoData;
        private DayData _dayData;
        private TodoDefData _todoDefData;
        private TimelineData _timelineData;

        public DayDetailViewModel(DayData dayData, TodoDefData todoDefData, TodoData todoData, TimelineData timelineData) 
        { 
            _dayData = dayData;
            _todoDefData = todoDefData;
            _todoData = todoData;
            _timelineData = timelineData;

            _todoDefData.TodoDefItemsChanged += onTodoDefsChanged;
            _todoData.TodoItemCollectionChanged += onTodoItemsChanged;
        }

        public async void onTodoDefsChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _todoDefData.GetTodoDefItems(TodoDefs);
                DefSearchText = string.Empty;
            });
        }

        public void onTodoItemsChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _todoData.GetTodos(TodoItems, myDayItem.Date);
                CalculateDailyPercentages();
            });
        }

        public void loadTimelineItems()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _timelineData.GetItemsByDate(TimelineItems, myDayItem.Date);
            });
        }

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

        public ICommand TodoAddCommand => new Command<TodoDefItem>((tdi) => { if (tdi == null) return; tdi.CreateTodoItem(_todoData, _myDayItem.Date.Date); });

        public ICommand DeleteCommand => new Command(async () => { await _dayData.DeleteItem(myDayItem.Id); await Shell.Current.GoToAsync(".."); });

        public ICommand NavigateToTodoDefListView => new Command(async () =>
        {
            await Shell.Current.GoToAsync("TodoDefListView");
        });


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

        public void CalculateDailyPercentages()
        {
            StatItems.Clear();
            var myList = new List<StatItem>();
            TimeSpan TotalDuration = myDayItem.Date == DateTime.Today ? DateTime.Now.TimeOfDay : TimeSpan.FromDays(1);
            TimeSpan ActiveDuration = TimeSpan.Zero;
            foreach (var item in TodoItems)
            {
                if (item.TotalTime == 0) continue;
                ActiveDuration += TimeSpan.FromSeconds(item.TotalTime);
                myList.Add(new StatItem
                {
                    Text = item.TodoText + ": " + TimeSpan.FromSeconds(item.TotalTime).ToString(@"hh\:mm\:ss") + " (" + (TimeSpan.FromSeconds(item.TotalTime).TotalHours / TotalDuration.TotalHours * 100).ToString("0.00") + "%)",
                    Color = item.Color,
                    Percentage = TimeSpan.FromSeconds(item.TotalTime).TotalHours / TotalDuration.TotalHours * 100
                });
            }
            foreach (var item in myList.OrderBy(x => x.Percentage))
            {
                StatItems.Add(item);
            }

            StatItems.Add(new StatItem
            {
                Text = "Recorded: " + ActiveDuration.ToString(@"hh\:mm\:ss") + " (" + (ActiveDuration.TotalHours / TotalDuration.TotalHours * 100).ToString("0.00") + "%)",
                Color = Colors.Green.ToHex(),
                Percentage = ActiveDuration.TotalHours / TotalDuration.TotalHours * 100
            });
            StatItems.Add(new StatItem
            {
                Text = "Offrecord: " + (TotalDuration - ActiveDuration).ToString(@"hh\:mm\:ss") + " (" + ((TotalDuration - ActiveDuration).TotalHours / TotalDuration.TotalHours * 100).ToString("0.00") + "%)",
                Color = Colors.Red.ToHex(),
                Percentage = ((TotalDuration - ActiveDuration).TotalHours / TotalDuration.TotalHours * 100)
            });
        }

        public class StatItem
        {
            public string Text { get; set; }
            public string Color { get; set; }
            public double Percentage { get; set; }
        }
    }
}
