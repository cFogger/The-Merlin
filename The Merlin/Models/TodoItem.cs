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
        public int Status { get; set; } = 0; // 0 Pending 1 InProgress 2 Success 3 Failed/Delayed
        public DateTime CreatedAt { get; set; }
        public bool AutoCreated { get; set; } = false;
        public DateTime AssignedDate { get; set; }
        public int ParentId { get; set; } = 0;
        public TimeSpan RepeatSpan { get; set; } = TimeSpan.Zero;
        public int Priority { get; set; } = 0; // 0 Low 1 Medium 2 High 3 Critical
        //public int TypeOfTodo { get; set; } = 0 // 0 OneTime 1 Repeats

        // 0 No Time
        // 1 Predefined (time has to be defined starts a countdown and when it ends ask do you wanna keep going or another round)
        // 2 EstimatedTime (starts a chronometer takes times average)
        // 3 LimitedTime (starts a countdown and when it ends notify to end this activity)
        public int TimeMode { get; set; } = 0;
        public TimeSpan Time { get; set; } = TimeSpan.Zero;
        public int DoneCount { get; set; } = 0;
        public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
    }
}
