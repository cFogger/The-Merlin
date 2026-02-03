using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(myItem), "dayitem")]
    public class DayDetailViewModel : BaseViewModel
    {
        public DayItem myItem { get { return _myItem; } set { 
                _myItem = value; 
                OnPropertyChanged(); 
                _dayData.GetTodos(TodoItems, myItem.Date);
            }
        }
        private DayItem _myItem;

        public ObservableCollection<TodoItem> TodoItems { get; } = [];

        private DayData _dayData;
        public DayDetailViewModel(DayData dayData) 
        { 
            _dayData = dayData;
        }

    }
}
