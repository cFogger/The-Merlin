using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace The_Merlin.Models
{
    public class DayItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DayType DayType { get; set; }
        [Ignore]
        public ObservableCollection<TodoItem> TodoItems { get; set; } = [];
    }

    public enum DayType
    {
        WorkDay,
        HomeDay,
        OffDay,
    }
}
