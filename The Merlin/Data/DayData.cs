using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class DayData
    {
        DataManager dtm;
        string prefix = "DayItems";
        public DayData(DataManager _dtm)
        {
            dtm = _dtm;
        }

        public async Task<DayItem> GetItem(DateTime? date = null)
        {
            if (date == null)
                date = DateTime.Today;

            return (DayItem)await dtm.resolveRespond(prefix + "/GetItem?date=" + date.Value.ToBinary());
        }

        public async Task GetAllDays(ObservableCollection<DayItem> myObs)
        {
            myObs.Clear();
            object templist = await dtm.resolveRespond(prefix + "/GetAll");
            List<DayItem> mylist = JsonConvert.DeserializeObject<List<DayItem>>(templist.ToString());
            foreach (DayItem item in mylist)
                myObs.Add(item);
        }

        public async Task SaveItem(DayItem item)
        {
            await dtm.resolveRespond(prefix + "/Save", JsonConvert.SerializeObject(item));
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteItem(int id)
        {
            await dtm.resolveRespond(prefix + "/Delete?id=" + id);
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? ItemChanged;
    }
}
