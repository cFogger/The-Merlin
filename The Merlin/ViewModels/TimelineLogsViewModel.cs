using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class TimelineLogsViewModel : BaseViewModel
    {
        public List<TimelineItem> TimelineIS { get { return _timelineIS; } set { _timelineIS = value; OnPropertyChanged(); } }
        private List<TimelineItem> _timelineIS;

        public TimelineLogsViewModel(DataManager dataManager)
        {
            TimelineIS = dataManager.TimelineData.GetAllItems();
            TimelineIS.Reverse();
        }
    }
}
