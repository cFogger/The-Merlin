using SQLite;

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
    }
}
