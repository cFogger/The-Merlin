using The_Merlin.Models;

namespace The_Merlin.Timer;

public partial class TimerView : ContentView
{
    Action<bool> saveTimelineItem;

    public TimerView(Action<bool> _saveTimelineItem)
    {
        InitializeComponent();
        StartButtonOrganize();
        saveTimelineItem = _saveTimelineItem;
        App.AppTimer.Tick += AppTimer_Tick;
    }

    private void AppTimer_Tick(object? sender, EventArgs e)
    {
        TimerLabel.Text = TimerService.ChronoTimer.ToString(@"hh\:mm\:ss");
    }

    private void StartButton_Clicked(object sender, EventArgs e)
    {
        if (TimerService.IsChronoRunning)
        {
            StartButtonOrganize();
            saveTimelineItem(true);
        }
        else
        {
            StartButtonOrganize(false);
            saveTimelineItem(false);
        }
        TimerService.IsChronoRunning = !TimerService.IsChronoRunning;
    }

    private void StartButtonOrganize(bool isStarted = true)
    {
        if (isStarted)
        {
            StartButton.Text = "▶";
            StartButton.BackgroundColor = Colors.GreenYellow;
            StartButton.TextColor = Colors.Black;
        }
        else
        {
            StartButton.Text = "■";
            StartButton.BackgroundColor = Color.FromArgb("FF1E00");
            StartButton.TextColor = Colors.AliceBlue;
        }
    }
}