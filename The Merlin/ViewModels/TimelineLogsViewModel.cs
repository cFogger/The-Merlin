using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class TimelineLogsViewModel : BaseViewModel
    {
        public ObservableCollection<TimelineItem> TimelineIS { get; } = [];

        private readonly TimelineData _timelineData;
        public TimelineLogsViewModel(TimelineData timelineData)
        {
            timelineData.TimelineChanged += onTimelineChanged;
            _timelineData = timelineData;
            onTimelineChanged(this, EventArgs.Empty);
        }

        private void onTimelineChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _timelineData.GetAllItems(TimelineIS);
            });
        }
    }
}
