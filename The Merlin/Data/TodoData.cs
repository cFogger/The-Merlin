using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using The_Merlin.Interfaces;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TodoData
    {
        private readonly DataManager dtm;
        private readonly DayData _dayData;
        public TodoData(DataManager _dtm, DayData dayData) { 
            dtm = _dtm; 
            _dayData = dayData; }
        
        public List<TodoItem> GetTodaysTodos() => [.. dtm.dbConnection.Table<TodoItem>().Where(x=>x.AssignedDate == DateTime.Today)];

        public List<TodoItem> GetItemsByTodoDefId(int tdi) => [.. dtm.dbConnection.Table<Models.TodoItem>().Where(x => x.TodoDefId == tdi)];

        public void GetAssignedDates()
        {
            var que = dtm.dbConnection.Query<Models.TodoItem>("SELECT DISTINCT AssignedDate FROM TodoItem");
            var dates = dtm.dbConnection.Query<DayItem>("SELECT DISTINCT Date FROM DayItem");
            foreach (var dt in que)
            {
                if (!dates.Exists(x => x.Date == dt.AssignedDate))
                    _dayData.AddItem(new DayItem
                    {
                        Date = dt.AssignedDate,
                        DayType = DayType.HomeDay,
                    });
            }

            if (dates.Exists(dates => dates.Date == DateTime.Today) == false)
                _dayData.AddItem(new DayItem
                {
                    Date = DateTime.Today,
                    DayType = DayType.HomeDay,
                }); 
            AssignedDatesChanged?.Invoke(this, EventArgs.Empty);
        }

        public TodoItem GetItem(int id)
        {
            return dtm.dbConnection.Table<Models.TodoItem>().FirstOrDefault(x => x.Id == id);
        }

        public void AddItem(TodoItem ti)
        {
            dtm.dbConnection.Insert(ti);
            TodoItemCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateItem(TodoItem ti)
        {
            dtm.dbConnection.Update(ti);
            TodoItemCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteItem(TodoItem ti)
        {
            dtm.dbConnection.Delete(ti);
            TodoItemCollectionChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public event EventHandler TodoItemCollectionChanged;
        public event EventHandler AssignedDatesChanged;
    }
}
