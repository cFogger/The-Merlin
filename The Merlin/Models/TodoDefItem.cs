using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using The_Merlin.Data;

namespace The_Merlin.Models
{
    public class TodoDefItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string? TodoDefText { get; set; } // Short description of the todo

        public TimeSpan RepeatSpan { get; set; } = TimeSpan.Zero;
        public TodoDefRepeatType RepeatType { get; set; } = 0; // 0 None, 1 Daily, 2 Weekly, 3 Custom

        public TimeSpan DefaultDuration { get; set; } = TimeSpan.FromMinutes(25);
        public TodoCompletionType DefaultCompletionType { get; set; } = TodoCompletionType.DurationBased;

        //public int CanBeDone { get; set; } = 0;
        //everywhere onlyhome atworkavailable nopc holiday

        [Ignore]
        public int RepeatEveryXDays
        {
            get
            {
                if (RepeatType == TodoDefRepeatType.Custom && RepeatSpan.TotalDays >= 1)
                    return (int)RepeatSpan.TotalDays;
                else
                    return 0;
            }
            set
            {
                if (RepeatType == TodoDefRepeatType.Custom && value >= 1)
                    RepeatSpan = TimeSpan.FromDays(value);
            }
        }

        [Ignore]
        public int GetDefaultDurationInMinutes
        {
            get
            {
                return (int)DefaultDuration.TotalMinutes;
            }
            set
            {
                if (value >= 1)
                    DefaultDuration = TimeSpan.FromMinutes(value);
            }
        }

        public void CreateTodoItem(TodoData _todoData)
        {
            TodoItem todoItem = new()
            {
                TodoDefId = this.Id,
                TodoText = this.TodoDefText,
                CreatedAt = DateTime.Now,
                AssignedDate = DateTime.Today,
                Status = TodoItemStatus.Pending,
                CompletionType = this.DefaultCompletionType,
                Duration = this.DefaultDuration
            };
            _todoData.AddItem(todoItem);
        }
    }

    public enum TodoDefRepeatType
    {
        None = 0,
        Daily = 1,
        Weekly = 2,
        Custom = 3
    }
}
