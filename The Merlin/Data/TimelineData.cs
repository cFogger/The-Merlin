using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Mime;
using System.Text;
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

        public TimelineItem GetItem(int id)
        {
            return dtm.dbConnection.Table<Models.TimelineItem>().FirstOrDefault(x => x.Id == id);
        }

        public void EndItem(int todoId, DateTime? ends)
        {
            var myItem = dtm.dbConnection.Table<Models.TimelineItem>().First(x => x.TodoId == todoId && x.Ends == null);
            myItem.Ends = ends;
            dtm.dbConnection.Update(myItem);
        }

        public void AddItem(TimelineItem ti)
        {
            dtm.dbConnection.Insert(ti);
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
