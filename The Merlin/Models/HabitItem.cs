using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Data;

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

        public async void AddHabitHistory(HabitHistoryData _habitData, int count = 1)
        {
            TotalCount += count; // Toplam sayıyı güncelle
            HabitHistoryItem historyItem = new()
            {
                HabitId = this.Id,
                HabitTime = DateTime.Now,
                Count = count
            };
            await _habitData.AddHabitHistoryItem(historyItem);
        }
    }
}
