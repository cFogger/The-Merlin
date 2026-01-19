using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TimelineData
    {
        private readonly DataManager dtm;
        public TimelineData(DataManager dataManager)
        {
            dtm = dataManager;
        }


        public List<TimelineItem> GetAllItems()
        {
            return dtm.dbConnection.Table<TimelineItem>().ToList();
        }

        public TimelineItem GetItem(int id)
        {
            return dtm.dbConnection.Table<Models.TimelineItem>().FirstOrDefault(x => x.Id == id);
        }

        public TimeSpan GetTotal(int todoId)
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

        public void EndItem(int todoId, DateTime? ends)
        {
            var myItem = dtm.dbConnection.Table<Models.TimelineItem>().First(x => x.TodoId == todoId && x.Ends == null);
            myItem.Ends = ends;
            dtm.dbConnection.Update(myItem);
        }

        public int AddItem(TimelineItem ti)
        {
            if (checkRunningTodo() != null)
            {
                return 0;
            }
            return dtm.dbConnection.Insert(ti);
        }

        public void UpdateItem(TimelineItem ti)
        {
            dtm.dbConnection.Update(ti);
        }

        public void DeleteItem(TimelineItem ti)
        {
            dtm.dbConnection.Delete(ti);
        }
    }
}
