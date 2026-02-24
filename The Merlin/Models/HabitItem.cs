using System;
using System.Collections.Generic;
using System.Text;

namespace The_Merlin.Models
{
    public class HabitItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int TotalCount { get; set; } // Genel toplam
        public int DailyMaxCount { get; set; } // Hedeflenen üst sınır
        public int DailyMinCount { get; set; } // Hedeflenen alt sınır
        public bool IsPositive { get; set; } // Olumlu (su) mu, Olumsuz (sigara) mı?
        public string Color { get; set; }

        // İlişki: Bir alışkanlığın birçok geçmiş kaydı olabilir
        public virtual List<HabitHistoryItem> History { get; set; } = new();
    }
}
