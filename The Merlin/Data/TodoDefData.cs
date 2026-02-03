using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TodoDefData
    {
        private readonly DataManager dtm;
        public TodoDefData(DataManager _dataManager)
        {
            dtm = _dataManager;
        }

        public List<TodoDefItem> GetAllTodoDefItems()
        {
            return dtm.dbConnection.Table<TodoDefItem>().ToList();
        }

        public TodoDefItem GetTodoDefItemById(int id)
        {
            return dtm.dbConnection.Table<TodoDefItem>().FirstOrDefault(t => t.Id == id);
        }

        public TimeSpan GetTotalDurationByTodoDefId(int tdi)
        {
            TimeSpan ts = TimeSpan.Zero;
            var todoItems = dtm.dbConnection.Table<TodoItem>().Where(x => x.TodoDefId == tdi).ToList();
            foreach (var ti in todoItems)
            {
                var timelines = dtm.dbConnection.Table<TimelineItem>().Where(x => x.TodoId == ti.Id && x.Ends != null).ToList();
                foreach (var tl in timelines)
                {
                    ts = ts.Add(tl.Ends.Value - tl.Starts);
                }
            }
            return ts;
        }

        public void AddTodoDefItem(TodoDefItem todoDef)
        {
            dtm.dbConnection.Insert(todoDef);
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateTodoDefItem(TodoDefItem todoDef)
        {
            dtm.dbConnection.Update(todoDef);
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteTodoDefItem(TodoDefItem todoDef)
        {
            dtm.dbConnection.Delete(todoDef);
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TodoDefItemsChanged;
    }
}
