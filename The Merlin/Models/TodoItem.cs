namespace The_Merlin.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public int TodoDefId { get; set; }
        public string? TodoText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AssignedDate { get; set; }
        public TodoItemStatus Status { get; set; } = 0;
        public TodoCompletionType CompletionType { get; set; }
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;
        public string Color { get; set; }

        public string TodoDefText { get; set; }
        public double TotalTime { get; set; }

        public bool IsCompleted
        {
            get
            {
                return this.Status == TodoItemStatus.Completed;
            }
        }

        public bool isDifferentFromDef
        {
            get
            {
                return TodoDefText != TodoText;
            }
        }

        public string getTotalTimeString
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(TotalTime);
                if (ts.TotalMinutes >= 60)
                    return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
                else if (ts.TotalMinutes >= 1)
                    return string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
                else
                    return string.Format("00:{0:D2}", ts.Seconds);
            }
        }
    }

    public enum TodoItemStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Failed = 3
    }

    public enum TodoCompletionType
    {
        Manual = 0,
        DurationBased = 1
    }
}
