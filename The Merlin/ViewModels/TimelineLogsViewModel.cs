using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using The_Merlin.CustomControls;
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

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; } set { _selectedDate = value; OnPropertyChanged(); onTimelineChanged(this, EventArgs.Empty);  }
        }

        public TimelineLogsViewModel(TimelineData timelineData)
        {
            timelineData.TimelineChanged += onTimelineChanged;
            _timelineData = timelineData;
            SelectedDate = DateTime.Today;
        }

        private void onTimelineChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _timelineData.GetItemsByDate(TimelineIS, SelectedDate);
            });
        }

        public ICommand DeleteCommand => new Command<TimelineItem>(async (item) =>
        {
            await _timelineData.DeleteItem(item.Id);
        });

        public ICommand EditCommand => new Command<TimelineItem>(async (item) =>
        {
            var popupresult = await Shell.Current.CurrentPage.ShowPopupAsync<TimelineItem>(new TimelineItemDetailPopup(item));
            if (popupresult != null && popupresult.Result != null)
            {
                await _timelineData.SaveItem(popupresult.Result);
                item = popupresult.Result;
            }
        });

        public ICommand TodayCommand => new Command(() =>
        {
            SelectedDate = DateTime.Today;
        });
    }
}
