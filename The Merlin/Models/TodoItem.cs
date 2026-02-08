namespace The_Merlin.Models
{
    public class TodoItem
    {

        public int Id { get; set; }
        public int TodoDefId { get; set; }
        public string? TodoText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AssignedDate { get; set; }
        public TodoItemStatus Status { get; set; } = 0; // 0 Pending 1 InProgress 2 Success 3 Failed/Delayed
        public TodoCompletionType CompletionType { get; set; }
        public TimeSpan Duration { get; set; } = TimeSpan.Zero; // Used if CompletionType is DurationBased

        public bool IsCompleted
        {
            get
            {
                return this.Status == TodoItemStatus.Completed;
            }
        }
    }

    public enum TodoItemStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }

    public enum TodoCompletionType
    {
        Manual = 0,
        DurationBased = 1
    }
}
