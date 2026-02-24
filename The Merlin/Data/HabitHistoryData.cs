using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class HabitHistoryData
    {
        private readonly DataManager dtm;
        private string prefix = "HabitHistory";
        public HabitHistoryData(DataManager _dtm)
        {
            dtm = _dtm;
        }

        public async Task<List<HabitHistoryItem>> GetAllHabitHistoryItems()
        {
            var xd = await dtm.resolveRespond(prefix + "/GetAll");
            return JsonConvert.DeserializeObject<List<HabitHistoryItem>>(xd.ToString());
        }

        public async Task GetHabitHistoryItems(ObservableCollection<HabitHistoryItem> myObs)
        {
            myObs.Clear();
            var mylist = await GetAllHabitHistoryItems();
            foreach (var item in mylist)
                myObs.Add(item);
        }

        public async Task AddHabitHistoryItem(HabitHistoryItem historyItem)
        {
            await dtm.resolveRespond(prefix + "/Save", JsonConvert.SerializeObject(historyItem));
            HabitHistoryItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteHabitHistoryItem(int id)
        {
            await dtm.resolveRespond(prefix + "/Delete?id=" + id);
            HabitHistoryItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler HabitHistoryItemsChanged;
    }
}
