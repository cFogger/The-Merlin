namespace The_Merlin.Timer;

public partial class ChronoView : ContentView
{
    IDispatcherTimer myTimer = App.AppTimer;

    public ChronoView()
    {
        InitializeComponent();
        if (App.ChronoTimer != TimeSpan.Zero)
        {
            StartButtonOrganize(false);
            if (App.IsChronoRunning)
            {
                TimerLabel.Text = App.ChronoTimer.ToString(@"hh\:mm\:ss");
            }
            else
            {
                TimerLabel.Text = App.ChronoTimer.ToString(@"hh\:mm\:ss");
                PauseButtonOrganize(true);
            }
        }
        else
        {
            StartButtonOrganize();
        }

        myTimer.Tick += OnTimedEvent;
    }

    private void OnTimedEvent(object? sender, EventArgs e)
    {
        TimerLabel.Text = App.ChronoTimer.ToString(@"hh\:mm\:ss");
    }

    private void StartButton_Clicked(object sender, EventArgs e)
    {
        if (App.IsChronoRunning)
        {
            App.IsChronoRunning = false;
            App.ChronoTimer = TimeSpan.Zero;
            StartButtonOrganize();
        }
        else
        {
            App.IsChronoRunning = true;
            StartButtonOrganize(false);
        }
    }

    private void PauseButton_Clicked(object sender, EventArgs e)
    {
        if (App.IsChronoRunning)
        {
            App.IsChronoRunning = false;
            PauseButtonOrganize(true);
        }
        else
        {
            App.IsChronoRunning = true;
            PauseButtonOrganize(false);
        }
    }

    private void ResetButton_Clicked(object sender, EventArgs e)
    {
        App.ChronoTimer = TimeSpan.Zero;
        TimerLabel.Text = App.ChronoTimer.ToString(@"hh\:mm\:ss");
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

    private void StartButtonOrganize(bool isStarted = true)
    {
        if (isStarted)
        {
            StartButton.Text = "▶";
            PauseButton.IsVisible = false;
            ResetButton.IsVisible = false;
            StartButton.SetValue(Grid.ColumnSpanProperty, 3);
            StartButton.BackgroundColor = Colors.GreenYellow;
            StartButton.TextColor = Colors.Black;
        }
        else
        {
            StartButton.Text = "■";
            PauseButton.IsVisible = true;
            ResetButton.IsVisible = true;
            StartButton.SetValue(Grid.ColumnSpanProperty, 1);
            StartButton.BackgroundColor = Color.FromArgb("FF1E00");
            StartButton.TextColor = Colors.AliceBlue;
        }
    }
}