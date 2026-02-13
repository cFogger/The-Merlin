using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Models;
using The_Merlin.Services;

namespace The_Merlin.ViewModels
{
    public class TimelineLogsViewModel : BaseViewModel
    {
        public ObservableCollection<TimelineItem> TimelineIS { get; } = [];

        private readonly TimelineData _timelineData;
        public IDispatcherTimer myDispatcher { get { return _timerService.Dispatcher(); } }
        private ITimerService _timerService;

        public TimelineLogsViewModel(TimelineData timelineData, ITimerService timerService)
        {
            timelineData.TimelineChanged += onTimelineChanged;
            _timelineData = timelineData;
            _timerService = timerService;
            onTimelineChanged(this, EventArgs.Empty);
        }

        private void onTimelineChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _timelineData.GetAllItems(TimelineIS);
            });
        }

        public ICommand DeleteCommand => new Command<TimelineItem>(async (item) =>
        {
            await _timelineData.DeleteItem(item.Id);
        });
    }
}
