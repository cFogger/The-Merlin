using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace The_Merlin.Models
{
    public class DayItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DayType DayType { get; set; }

        public ObservableCollection<TodoItem> TodoItems { get; set; } = [];
    }

    public enum DayType
    {
        WorkDay=0,
        HomeDay=1,
        OffDay=2,
    }
}
