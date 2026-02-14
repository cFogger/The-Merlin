using The_Merlin.Data;

namespace The_Merlin.Models
{
    public class TimelineItem
    {

        public int Id { get; set; }
        public int TodoId { get; set; }
        public string Context { get; set; }
        public DateTime Starts { get; set; } = DateTime.Now;
        public DateTime? Ends { get; set; }

        public string TodoColor { get; set; }
        public string TodoName { get; set; }

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

        public string GetStartString
        {
            get
            {
                if (Starts.Date == DateTime.Today)
                    return Starts.ToShortTimeString();
                else
                    return Starts.ToString();
            }
        }

        public string GetEndString
        {
            get
            {
                if (Ends.HasValue)
                    if (Ends.Value.Date == DateTime.Today)
                        return Ends.Value.ToShortTimeString();
                    else
                        return Ends.Value.ToString();
                else
                    return "...";
            }
        }

        public bool IsToday
        {
            get
            {
                return Starts.Date == DateTime.Now.Date || (Ends.HasValue && Ends.Value.Date == DateTime.Today);
            }
        }

        public Color Color { get { return Color.FromArgb(TodoColor); } }
    }
}
