using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public List<TimelineItem> TimelineIS { get { return _timelineIS; } set { _timelineIS = value; OnPropertyChanged(); } }
        private List<TimelineItem> _timelineIS;

        public MainPageViewModel(DataManager dataManager)
        {
            TimelineIS = dataManager.TimelineData.GetLastxItems(5);
        }
    }
}
