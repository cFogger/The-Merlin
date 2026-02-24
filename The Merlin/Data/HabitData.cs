using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class HabitData
    {
        private readonly DataManager dtm;
        private string prefix = "HabitItems";
        public HabitData(DataManager _dtm)
        {
            dtm = _dtm;
        }

        public async Task<List<HabitItem>> GetAllHabitItems()
        {
            var xd = await dtm.resolveRespond(prefix + "/GetAll");
            return JsonConvert.DeserializeObject<List<HabitItem>>(xd.ToString());
        }

        public async Task GetHabitItems(ObservableCollection<HabitItem> myObs)
        {
            myObs.Clear();
            var mylist = await GetAllHabitItems();
            foreach (var item in mylist)
                myObs.Add(item);
        }

        public async Task AddHabitItem(HabitItem habit)
        {
            await dtm.resolveRespond(prefix + "/Save", JsonConvert.SerializeObject(habit));
            HabitItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteTodoDefItem(int id)
        {
            await dtm.resolveRespond(prefix + "/Delete?id=" + id);
            HabitItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler HabitItemsChanged;
    }
}
