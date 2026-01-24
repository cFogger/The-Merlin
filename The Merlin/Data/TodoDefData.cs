using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TodoDefData
    {
        private readonly DataManager dataManager;
        public TodoDefData(DataManager _dataManager)
        {
            dataManager = _dataManager;
        }

        public List<TodoDefItem> GetAllTodoDefItems()
        {
            return dataManager.dbConnection.Table<TodoDefItem>().ToList();
        }

        public void AddTodoDefItem(TodoDefItem todoDef)
        {
            dataManager.dbConnection.Insert(todoDef);
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateTodoDefItem(TodoDefItem todoDef)
        {
            dataManager.dbConnection.Update(todoDef);
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteTodoDefItem(TodoDefItem todoDef)
        {
            dataManager.dbConnection.Delete(todoDef);
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TodoDefItemsChanged;
    }
}
