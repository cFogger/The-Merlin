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

        // Alışkanlığın tam olarak ne zaman gerçekleştiği (İstatistik için kritik)
        public DateTime HabitTime { get; set; } = DateTime.Now;

        // O an eklenen miktar (Örn: 1 bardak, 200ml, 1 adet vb.)
        public int Count { get; set; }

        // Geriye dönük erişim için navigation property
        [JsonIgnore] // API döngüsünü engellemek için
        public virtual HabitItem Habit { get; set; }
    }
}
