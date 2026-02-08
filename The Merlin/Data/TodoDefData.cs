using Newtonsoft.Json;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class TodoDefData
    {
        private readonly DataManager dtm;
        private string prefix = "TodoDefItems";

        public TodoDefData(DataManager _dataManager)
        {
            dtm = _dataManager;
        }

        public async Task<List<TodoDefItem>> GetAllTodoDefItems()
        {
            var xd = await dtm.resolveRespond(prefix + "/GetAll");
            return JsonConvert.DeserializeObject<List<TodoDefItem>>(xd.ToString());
        }

        public async Task<TodoDefItem> GetTodoDefItemById(int id)
        {
            var temp = await dtm.resolveRespond(prefix + "/GetItem?id=" + id);
            return JsonConvert.DeserializeObject<TodoDefItem>(temp.ToString());
        }

        public async Task<TimeSpan> GetTotalDurationByTodoDefId(int id)
        {
            var test = await dtm.resolveRespond(prefix + "/GetTotalByTodoDef?id=" + id);
            var tst2 = TimeSpan.Parse(test.ToString());
            return tst2;
        }

        public async Task AddTodoDefItem(TodoDefItem todoDef)
        {
            await dtm.resolveRespond(prefix + "/Save", JsonConvert.SerializeObject(todoDef));
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteTodoDefItem(int id)
        {
            await dtm.resolveRespond(prefix + "/Delete?id=" + id);
            TodoDefItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TodoDefItemsChanged;
    }
}
