using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_Merlin.Models
{
    public class HabitHistoryItem
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime HabitTime { get; set; }
        public int Count { get; set; }
        public string HabitTitle { get; set; }
        public string HabitColor { get; set; }
    }
}
