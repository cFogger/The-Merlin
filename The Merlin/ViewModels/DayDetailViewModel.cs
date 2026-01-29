using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(myItem), "dayitem")]
    public class DayDetailViewModel : BaseViewModel
    {
        public DayItem myItem { get { return _myItem; } set { _myItem = value; OnPropertyChanged(); } }
        private DayItem _myItem;
        public DayDetailViewModel() { }
    }
}
