using SQLite;

namespace The_Merlin.Models
{
    public class TodoItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int TodoDefId { get; set; }
        public string? TodoText { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AssignedDate { get; set; }
        public TodoItemStatus Status { get; set; } = 0; // 0 Pending 1 InProgress 2 Success 3 Failed/Delayed

        public int Priority { get; set; } = 0; // 0 Low 1 Medium 2 High 3 Critical
    }

    public enum TodoItemStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }
}
