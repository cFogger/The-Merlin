using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Merlin.Models
{
    public class DayItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DayType DayType { get; set; }
        public string Content { get; set; }
        public string HabitsJson { get; set; } // Veritabanında string tutulur

        [NotMapped]
        public List<HabitItem> Habits
        {
            get => JsonConvert.DeserializeObject<List<HabitItem>>(HabitsJson ?? "[]");
            set => HabitsJson = JsonConvert.SerializeObject(value);
        }
    }

    public enum DayType
    {
        WorkDay=0,
        HomeDay=1,
        OffDay=2,
    }
}
