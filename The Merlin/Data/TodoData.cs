using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TodoData
    {
        private readonly DataManager dtm;
        private readonly DayData _dayData;
        private string prefix = "TodoItems";
        public TodoData(DataManager _dtm, DayData dayData)
        {
            dtm = _dtm;
            _dayData = dayData;
        }

        public async Task GetTodos(ObservableCollection<TodoItem> myObs, DateTime? date = null)
        {
            if (date == null) { date = DateTime.Today; }
            myObs.Clear();
            var mylist = await GetItemsByDate(date.Value);
            foreach (TodoItem item in mylist)
                myObs.Add(item);
        }

        public async Task<List<TodoItem>> GetItemsByDate(DateTime date)
        {
            var xd = await dtm.resolveRespond(prefix + "/GetItemsByDate?date=" + date.Date.ToBinary());
            return JsonConvert.DeserializeObject<List<TodoItem>>(xd.ToString());
        }

        public async Task<List<TodoItem>> GetItemsByTodoDefId(int tdi)
        {
            var xd = await dtm.resolveRespond(prefix + "/GetItemsByTodoDefId?tdiid=" + tdi);
            return JsonConvert.DeserializeObject<List<TodoItem>>(xd.ToString());
        }

        public async Task GetAssignedDates()
        {
            await dtm.HttpClient.GetAsync(dtm.Url + prefix + "/GetAssignedDates");
            AssignedDatesChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<TodoItem> GetItem(int id)
        {
            var resolve = await dtm.resolveRespond(prefix + "/GetItem?id=" + id);
            var myobj = JsonConvert.DeserializeObject<TodoItem>(resolve.ToString());
            return myobj;
        }

        public async Task SaveItem(TodoItem item)
        {
            Debug.WriteLine("isitsave");
            await dtm.resolveRespond(prefix + "/Save", JsonConvert.SerializeObject(item));
            TodoItemCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteItem(int id)
        {
            await dtm.resolveRespond(prefix + "/Delete?id=" + id);
            TodoItemCollectionChanged.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? TodoItemCollectionChanged;
        public event EventHandler? AssignedDatesChanged;
    }
}
