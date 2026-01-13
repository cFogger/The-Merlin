using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TodoData
    {
        private readonly DataManager dtm;
        public TodoData(DataManager _dtm) { dtm = _dtm; GetAssignedDates(); }

        public List<TodoItem> GetUndoneItems(DateTime date) => [.. dtm.dbConnection.Table<Models.TodoItem>().Where(x => x.IsCompleted == false && x.AssignedDate == date)];

        public List<TodoItem> GetDoneItems(DateTime date) => [.. dtm.dbConnection.Table<Models.TodoItem>().Where(x => x.IsCompleted && x.AssignedDate == date)];

        public List<DateTime> GetAssignedDates()
        {
            var que = dtm.dbConnection.Query<Models.TodoItem>("SELECT DISTINCT AssignedDate FROM TodoItem");
            List<DateTime> dates = [];
            foreach (var dt in que)
            {
                if (!dates.Exists(x => x == dt.AssignedDate))
                    dates.Add(dt.AssignedDate);
            }
            if (dates.Exists(dates => dates == DateTime.Today) == false)
                dates.Add(DateTime.Today);
            return dates;
        }

        public TodoItem GetItem(int id)
        {
            return dtm.dbConnection.Table<Models.TodoItem>().FirstOrDefault(x => x.Id == id);
        }

        public void AddItem(TodoItem ti)
        {
            dtm.dbConnection.Insert(ti);
        }

        public void UpdateItem(TodoItem ti)
        {
            dtm.dbConnection.Update(ti);
        }

        public void DeleteItem(TodoItem ti)
        {
            dtm.dbConnection.Delete(ti);
        }
    }
}
