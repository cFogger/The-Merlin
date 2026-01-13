using SQLite;

namespace The_Merlin.Models
{
    public class TodoItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string? TodoText { get; set; }
        public string? TodoInside { get; set; }
        public bool IsCompleted { get; set; }
        public int Status { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public bool AutoCreated { get; set; } = false;
        public DateTime AssignedDate { get; set; }
        public int ParentId { get; set; } = 0;
        public TimeSpan RepeatSpan { get; set; } = TimeSpan.Zero;
        public int Priority { get; set; } = 0;
    }
}
