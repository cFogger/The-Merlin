using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Data
{
    public class DayData
    {
        DataManager dtm;
        public DayData(DataManager _dtm)
        {
            dtm = _dtm;
        }

        public void GetAllDays(ObservableCollection<DayItem> myObs)
        {
            myObs.Clear();
            var mylist = dtm.dbConnection.Table<DayItem>().ToList();
            foreach (DayItem item in mylist)
                myObs.Add(item);
        }

        public DayItem GetItem(DateTime? date = null)
        {
            if (date == null) { date = DateTime.Today; }
            return dtm.dbConnection.Table<DayItem>().First(x => x.Date == date.Value.Date);
        }

        public void AddItem(DayItem item)
        {
            if (dtm.dbConnection.Table<DayItem>().Count(x => x.Date == DateTime.Today) > 0)
                return;
            dtm.dbConnection.Insert(item);
            ItemChanged.Invoke(this, EventArgs.Empty);
        }

        public void UpdateItem(DayItem item)
        { dtm.dbConnection.Update(item); ItemChanged.Invoke(this, EventArgs.Empty); }
        public void DeleteItem(DayItem item)
        { dtm.dbConnection.Delete(item); ItemChanged.Invoke(this, EventArgs.Empty); }

        public event EventHandler ItemChanged;
    }
}
