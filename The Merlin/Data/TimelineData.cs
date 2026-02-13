using Newtonsoft.Json;
using System.Collections.ObjectModel;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TimelineData
    {
        private readonly DataManager dtm;
        string prefix = "TimelineItems";
        public TimelineData(DataManager dataManager)
        {
            dtm = dataManager;
        }

        public async Task GetAllItems(ObservableCollection<TimelineItem> timelinecoll)
        {
            timelinecoll.Clear();
            var temp = await dtm.resolveRespond(prefix + "/GetAll");
            var allItems = JsonConvert.DeserializeObject<List<TimelineItem>>(temp.ToString());
            foreach (var item in allItems)
            {
                timelinecoll.Add(item);
            }
        }

        public async Task GetItemsByDate(ObservableCollection<TimelineItem> timelinecoll, DateTime? _date = null)
        {
            timelinecoll.Clear();
            string myUrl = _date.HasValue ? "/GetTimelineItemsByDate?date=" + _date.Value.ToBinary() : "/GetTimelineItemsByDate";
            var temp = await dtm.resolveRespond(prefix + myUrl);
            var allItems = JsonConvert.DeserializeObject<List<TimelineItem>>(temp.ToString());
            foreach (var item in allItems)
            {
                timelinecoll.Add(item);
            }
        }

        public async Task GetLastxItems(ObservableCollection<TimelineItem> timelinecoll, int Count)
        {
            timelinecoll.Clear();
            var temp = await dtm.resolveRespond(prefix + "/GetLastX?count=" + Count);
            var items = JsonConvert.DeserializeObject<List<TimelineItem>>(temp.ToString());
            if (items != null)
                foreach (var item in items)
                {
                    timelinecoll.Add(item);
                }
        }

        public async Task<TimelineItem> GetItem(int id)
        {
            return (TimelineItem)await dtm.resolveRespond(prefix + "/GetItem?id=" + id);
        }

        public async Task<TimeSpan> GetTotalbyTodoId(int todoId)
        {
            var respnd = await dtm.resolveRespond(prefix + "/GetTotalbyTodoId?id=" + todoId);
            return TimeSpan.Parse(respnd.ToString());
        }

        public async Task<TimelineItem?> checkRunningTodo()
        {
            var test = await dtm.resolveRespond(prefix + "/checkRunningTodo");

            if (test != null)
                return JsonConvert.DeserializeObject<TimelineItem?>(test.ToString());
            else
                return null;
        }

        public async Task EndItem(int todoId, DateTime? ends, string? context = null)
        {
            var myItem = new TimelineItem();
            myItem.TodoId = todoId;
            myItem.Ends = ends;
            myItem.Context = context;
            await dtm.resolveRespond(prefix +"/End", JsonConvert.SerializeObject(myItem));
            TimelineChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<int> SaveItem(TimelineItem ti)
        {
            var temp = await dtm.resolveRespond(prefix + "/Save", JsonConvert.SerializeObject(ti));
            TimelineChanged?.Invoke(this, EventArgs.Empty);

            if (temp != null)
                return 1;
            else
                return 0;
        }

        public async Task DeleteItem(int id)
        {
            await dtm.resolveRespond(prefix + "/Delete?id=" + id);
            TimelineChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TimelineChanged;
    }
}
