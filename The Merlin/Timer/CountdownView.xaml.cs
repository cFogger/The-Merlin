using The_Merlin.Models;

namespace The_Merlin.Timer;

public partial class CountdownView : ContentView
{
    bool todoMode = false;
    TodoItem item;

	public CountdownView(TodoItem TodoCnt, Action<TimelineItem> saveTimelineItem)
	{
		InitializeComponent();
        App.CountdownTimer = TodoCnt.Time;
        App.IsCntDwnRunning = false;
        TimerLabel.Text = App.CountdownTimer.ToString(@"hh\:mm\:ss");
        StartButtonOrganize();
        todoMode = true;
        item = TodoCnt;
        App.AppTimer.Tick += AppTimer_Tick;
    }

    public CountdownView()
    {
        InitializeComponent();
        if (App.CountdownTimer != TimeSpan.Zero)
        {
            StartButtonOrganize(false);
            if (App.IsCntDwnRunning)
            {
                TimerLabel.Text = App.CountdownTimer.ToString(@"hh\:mm\:ss");
            }
            else
            {
                TimerLabel.Text = App.CountdownTimer.ToString(@"hh\:mm\:ss");
                PauseButtonOrganize(true);
            }
        }
        else
        {
            StartButtonOrganize();
        }

        App.AppTimer.Tick += AppTimer_Tick; ;
    }

    private void AppTimer_Tick(object? sender, EventArgs e)
    {
        TimerLabel.Text = App.CountdownTimer.ToString(@"hh\:mm\:ss");
    }

    private void IncreaseTimeBtn_Clicked(object sender, EventArgs e)
    {
        App.CountdownTimer = App.CountdownTimer.Add(TimeSpan.FromMinutes(1));
        TimerLabel.Text = App.CountdownTimer.ToString(@"hh\:mm\:ss");
        if (todoMode)
        {
            item.Time = App.CountdownTimer;
            App.DataManager.TodoData.UpdateItem(item);
        }

    }

    private void DecreaseTimeBtn_Clicked(object sender, EventArgs e)
    {
        App.CountdownTimer = App.CountdownTimer - TimeSpan.FromMinutes(1) < TimeSpan.Zero ? TimeSpan.Zero : App.CountdownTimer.Add(TimeSpan.FromMinutes(-1));
        TimerLabel.Text = App.CountdownTimer.ToString(@"hh\:mm\:ss");
        if (todoMode)
        {
            item.Time = App.CountdownTimer;
            App.DataManager.TodoData.UpdateItem(item);
        }
    }

    private void StartButton_Clicked(object sender, EventArgs e)
    {
        if (App.IsCntDwnRunning)
        {
            App.IsCntDwnRunning = false;
            App.CountdownTimer = TimeSpan.Zero;
            StartButtonOrganize();
        }
        else
        {
            if (App.CountdownTimer > TimeSpan.Zero)
            {
                App.IsCntDwnRunning = true;
                StartButtonOrganize(false);
            }
        }
    }

    private void StartButtonOrganize(bool isStarted = true)
    {
        if (isStarted)
        {
            StartButton.Text = "▶";
            PauseButton.IsVisible = false;
            StartButton.SetValue(Grid.ColumnSpanProperty, 3);
            StartButton.BackgroundColor = Colors.GreenYellow;
            StartButton.TextColor = Colors.Black;
        }
        else
        {
            StartButton.Text = "■";
            PauseButton.IsVisible = true;
            StartButton.SetValue(Grid.ColumnSpanProperty, 1);
            StartButton.BackgroundColor = Color.FromArgb("FF1E00");
            StartButton.TextColor = Colors.AliceBlue;
        }
    }

    private void PauseButtonOrganize(bool isPaused = false)
    {
        if (isPaused)
        {
            PauseButton.Text = "▶";
            PauseButton.BackgroundColor = Colors.GreenYellow;
            StartButton.IsEnabled = false;
            StartButton.IsVisible = false;
        }
        else
        {
            PauseButton.Text = "Ⅱ";
            PauseButton.BackgroundColor = Colors.Orange;
            StartButton.IsEnabled = true;
            StartButton.IsVisible = true;
        }
    }

    private void PauseButton_Clicked(object sender, EventArgs e)
    {
        if (App.IsCntDwnRunning)
        {
            App.IsCntDwnRunning = false;
            PauseButtonOrganize(true);
        }
        else
        {
            App.IsCntDwnRunning = true;
            PauseButtonOrganize(false);
        }
    }
}