using System.Collections.ObjectModel;
using System.Diagnostics;
using The_Merlin.CustomControls;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<TimelineItem> TimelineIS { get; } = new ObservableCollection<TimelineItem>();

        public string dateString { get { return DateTime.Today.ToString("dd.MM.yy"); } }

        public List<View> todoViews { get { return _todoViews; } set { _todoViews = value; OnPropertyChanged(); } }
        private List<View> _todoViews;


        private TimelineData _timelineData;
        private TodoData _todoData;

        public MainPageViewModel(TimelineData timelineData, TodoData todoData)
        {
            _timelineData = timelineData; 
            _todoData = todoData;

            _timelineData.TimelineChanged += onTimelineChanged;
            ReloadLast5();
            ReloadTodos();
        }

        public void onTimelineChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(ReloadLast5);
        }

        public void ReloadLast5()
        {
            var items = _timelineData.GetLastxItems(5);

            TimelineIS.Clear();
            foreach (var item in items)
                TimelineIS.Add(item);
        }

        public void ReloadTodos()
        {
            todoViews = new List<View>();
            DateTime selectedDate = DateTime.Today;

            foreach (var item in _todoData.GetUndoneItems(selectedDate))
                todoViews.Add(new TodoView(item));

            foreach (var item in _todoData.GetDoneItems(selectedDate))
                todoViews.Add(new TodoView(item));

            todoViews.Add(new TodoAdd(ReloadTodos));
        }
    }
}
