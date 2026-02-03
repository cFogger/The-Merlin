using SQLite;
using System.Diagnostics;
using System.Windows.Input;

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
        public TodoCompletionType CompletionType { get; set; } = TodoCompletionType.Manual;
        public TimeSpan Duration { get; set; } = TimeSpan.Zero; // Used if CompletionType is DurationBased
        public int Priority { get; set; } = 0; // 0 Low 1 Medium 2 High 3 Critical
    }

    public enum TodoItemStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }

    public enum TodoCompletionType
    {
        Manual,
        DurationBased
    }
}
