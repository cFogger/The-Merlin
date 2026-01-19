using SQLite;
using The_Merlin.Data;

namespace The_Merlin.Models
{
    public class TimelineItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int TodoId { get; set; }
        public DateTime Starts { get; set; } = DateTime.Now;
        public DateTime? Ends { get; set; }

        [Ignore]
        public TimeSpan Duration
        {
            get
            {
                if (Ends != null)
                    return Ends.Value - Starts;
                else
                    return DateTime.Now - Starts;
            }
        }

        [Ignore]
        public string GetTodoName
        {
            get
            {
                TodoItem? td = Application.Current.Handler.MauiContext.Services.GetService<DataManager>().TodoData.GetItem(TodoId);
                if (td != null)
                    return td.TodoText;
                else
                    return "Deleted Todo";
            }
        }

        [Ignore]
        public string GetDurationString
        {
            get
            {
                TimeSpan ts = Duration;
                if (ts.TotalMinutes > 60)
                    return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
                else if (ts.TotalMinutes >= 1)
                    return string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
                else
                    return string.Format("00:{0:D2}", ts.Seconds);
            }
        }
    }
}
