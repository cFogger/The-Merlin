using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.CustomControls;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<TimelineItem> TimelineIS { get; } = [];
        public ObservableCollection<TodoItem> todoItems { get; } = [];
        public ObservableCollection<TodoDefItem> TodoDefs { get; } = [];

        public string dateString { get { return DateTime.Today.ToString("dd.MM.yy"); } }

        public List<View> todoViews { get { return _todoViews; } set { _todoViews = value; OnPropertyChanged(); } }
        private List<View> _todoViews;


        private TimelineData _timelineData;
        private TodoData _todoData;
        private TodoDefData _todoDefData;

        public MainPageViewModel(TimelineData timelineData, TodoData todoData, TodoDefData todoDefData)
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
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var items = _todoDefData.GetAllTodoDefItems();
                TodoDefs.Clear();
                foreach (var item in items)
                    TodoDefs.Add(item);
            });
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

        public ICommand TodoAddCommand => new Command(() => { SelectedTodoDef.CreateTodoItem(_todoData); });
        public TodoDefItem SelectedTodoDef { get; set; }
    }
}
