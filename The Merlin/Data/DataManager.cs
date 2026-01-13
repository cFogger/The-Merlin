using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace The_Merlin.Data
{
    public class DataManager
    {
        public readonly SQLiteConnection dbConnection;
        public TodoData TodoData { get { return new TodoData(this); } }
        public DataManager()
        {
            dbConnection = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "themerlin.db3"));
            Debug.WriteLine($"Database path: {Path.Combine(FileSystem.AppDataDirectory, "themerlin.db3")}");
            dbConnection.CreateTable<Models.TodoItem>();
        }
    }
}
