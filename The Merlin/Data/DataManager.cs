using SQLite;
using System.Diagnostics;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class DataManager
    {
        public readonly SQLiteConnection dbConnection;
        public DataManager()
        {
            dbConnection = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "themerlin.db3"));
            Debug.WriteLine($"Database path: {Path.Combine(FileSystem.AppDataDirectory, "themerlin.db3")}");
            dbConnection.CreateTable<TodoItem>();
            dbConnection.CreateTable<TimelineItem>();
            dbConnection.CreateTable<TodoDefItem>();
            dbConnection.CreateTable<DayItem>();
        }
    }
}
