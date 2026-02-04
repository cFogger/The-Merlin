using System.Collections.ObjectModel;
using System.Diagnostics;
using The_Merlin.Models;
using The_Merlin.ViewModels;

namespace The_Merlin.Data
{
    public class TimelineData
    {
        private readonly DataManager dtm;
        public TimelineData(DataManager dataManager)
        {
            dtm = dataManager;
        }


        public void GetAllItems(ObservableCollection<TimelineItem> timelinecoll)
        {
            timelinecoll.Clear();
            var allItems = dtm.dbConnection.Table<TimelineItem>().OrderByDescending(x => x.Starts).ToList();
            foreach (var item in allItems)
            {
                timelinecoll.Add(item);
            }
        }

        public void GetLastxItems(ObservableCollection<TimelineItem> timelinecoll, int Count)
        {
            timelinecoll.Clear();
            var items = dtm.dbConnection.Table<TimelineItem>().OrderByDescending(x => x.Starts).Take(Count).ToList();
            foreach (var item in items)
            {
                timelinecoll.Add(item);
            }
        }

        public TimelineItem GetItem(int id)
        {
            return dtm.dbConnection.Table<TimelineItem>().FirstOrDefault(x => x.Id == id);
        }

        public TimeSpan GetTotalbyTodoId(int todoId)
        {
            TimeSpan ts = TimeSpan.Zero;
            List<TimelineItem> tli = dtm.dbConnection.Table<TimelineItem>().Where(x => x.TodoId == todoId && x.Ends != null).ToList();
            foreach (TimelineItem item in tli)
            {
                ts = ts.Add(item.Ends.Value - item.Starts);
            }
            return ts;
        }

        public TimeSpan GetTodays(int todoId, DateTime? date = null)
        {
            if (date == null)
                date = DateTime.Today;

            TimeSpan ts = TimeSpan.Zero;
            List<TimelineItem> tli = dtm.dbConnection.Table<TimelineItem>().Where(x => x.TodoId == todoId && x.Starts.Date == date.Value.Date && x.Ends != null).ToList();
            foreach (TimelineItem item in tli)
                ts = ts.Add(item.Ends.Value - item.Starts);
            return ts;
        }

        public TimelineItem? checkRunningTodo()
        {
            return dtm.dbConnection.Table<TimelineItem>().Where(x => x.Ends == null).FirstOrDefault();
        }

        public int AddANoTimeItem(TimelineItem ti)
        {
            int xd = dtm.dbConnection.Insert(ti);
            TimelineChanged?.Invoke(this, EventArgs.Empty);
            return xd;
        }

        public void EndItem(int todoId, DateTime? ends, string? context = null)
        {
            var myItem = dtm.dbConnection.Table<Models.TimelineItem>().First(x => x.TodoId == todoId && x.Ends == null);
            myItem.Ends = ends;
            myItem.Context = context;
            dtm.dbConnection.Update(myItem);
            TimelineChanged?.Invoke(this, EventArgs.Empty);
        }

        public int AddItem(TimelineItem ti)
        {
            if (checkRunningTodo() != null)
            {
                return 0;
            }
            int xd = dtm.dbConnection.Insert(ti);
            TimelineChanged?.Invoke(this, EventArgs.Empty);
            return xd;
        }

        public void UpdateItem(TimelineItem ti)
        {
            dtm.dbConnection.Update(ti);
            TimelineChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteItem(TimelineItem ti)
        {
            Debug.WriteLine("Deleting Timeline Item ID " + ti.Id);
            dtm.dbConnection.Delete(ti);
            TimelineChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TimelineChanged;
    }
}
