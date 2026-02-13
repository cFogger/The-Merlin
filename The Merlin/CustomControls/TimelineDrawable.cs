using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.CustomControls
{
    public class TimelineDrawable : IDrawable
    {
        public List<TimelineItem> Items { get; set; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            double widthPerHour = dirtyRect.Width / 24;

            foreach (var item in Items)
            {
                // Başlangıç saati (Örn: 14:30 -> 14.5)
                double startPos = item.Starts.TimeOfDay.TotalHours;

                // Bitiş saati (Eğer null ise şu anki saati al ki aktif görev de çizilsin)
                double endPos = (item.Ends ?? DateTime.Now).TimeOfDay.TotalHours;

                // Eğer görev bir günden uzun sürüyorsa veya ertesi güne sarkıyorsa 
                // sadece bugün içindeki kısmını çizmek için:
                //if (!item.IsToday && item.Starts.Date < DateTime.Now.Date) startPos = 0;
                //if (item.Ends.HasValue && item.Ends.Value.Date > item.Starts.Date) endPos = 24;

                float x = (float)(startPos * widthPerHour);
                float w = (float)((endPos - startPos) * widthPerHour);

                // Kutucuk çizimi
                canvas.FillColor = item.Color; // TodoId'ye göre renk döndüren bir fonksiyon
                canvas.FillRoundedRectangle(x, 15, w, 35, 4);

                // Eğer kutu yeterince genişse içine süreyi yazalım
                if (w > 40)
                {
                    canvas.FontColor = Colors.White;
                    canvas.FontSize = 9;
                    canvas.DrawString(item.GetDurationString, x + 2, 35, HorizontalAlignment.Left);
                }
            }

            canvas.StrokeColor = canvas.FontColor = Colors.White;

            for (int i = 0; i < 24; i++)
            {
                float calcWidth = (float)(i * widthPerHour);

                // Saat çizgisi (Kalın ve Uzun)
                canvas.StrokeSize = 3;
                canvas.DrawLine(calcWidth, 50, calcWidth, 30);

                // Yarım saat çizgisi (Orta)
                canvas.StrokeSize = 2;
                float halfHour = (float)(calcWidth - (widthPerHour / 2));
                canvas.DrawLine(halfHour, 50, halfHour, 35);

                // Çeyrek saat çizgileri (İnce ve Kısa)
                canvas.StrokeSize = 1;
                canvas.DrawLine((float)(calcWidth - (widthPerHour / 4) * 3), 50, (float)(calcWidth - (widthPerHour / 4) * 3), 40);
                canvas.DrawLine((float)(calcWidth - (widthPerHour / 4)), 50, (float)(calcWidth - (widthPerHour / 4)), 40);

                // Saat Etiketi (Label yerine doğrudan Canvas üzerine yazı)
                canvas.FontSize = 8;
                canvas.DrawString(i.ToString(), calcWidth, 25, HorizontalAlignment.Center);
            }

            DrawCurrentTimeLine(canvas, dirtyRect, widthPerHour);
        }

        private void DrawCurrentTimeLine(ICanvas canvas, RectF dirtyRect, double widthPerHour)
        {
            float nowPos = (float)(DateTime.Now.TimeOfDay.TotalHours * widthPerHour);
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            canvas.DrawLine(nowPos, 0, nowPos, dirtyRect.Height);
        }
    }

    public class TimelineView : GraphicsView
    {
        public static readonly BindableProperty ItemsProperty =
                BindableProperty.Create(
                    nameof(Items),
                    typeof(ObservableCollection<TimelineItem>),
                    typeof(TimelineView),
                    null,
                    propertyChanged: OnItemsChanged);

        public ObservableCollection<TimelineItem> Items
        {
            get => (ObservableCollection<TimelineItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TimelineView)bindable;

            // Eski listenin olay aboneliğini temizle
            if (oldValue is ObservableCollection<TimelineItem> oldList)
                oldList.CollectionChanged -= control.OnCollectionChanged;

            // Yeni listenin olaylarına abone ol
            if (newValue is ObservableCollection<TimelineItem> newList)
            {
                newList.CollectionChanged += control.OnCollectionChanged;
                if (control.Drawable is TimelineDrawable drawable)
                {
                    drawable.Items = newList.ToList();
                    control.Invalidate();
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Drawable is TimelineDrawable drawable)
            {
                drawable.Items = Items.ToList();
                Invalidate(); // Liste değiştiği anda (ekleme/çıkarma) yeniden çiz
            }
        }

        public TimelineView()
        {
            Drawable = new TimelineDrawable();
        }
    }
}
