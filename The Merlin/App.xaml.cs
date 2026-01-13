using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using The_Merlin.Data;

namespace The_Merlin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        public static DataManager DataManager = new DataManager();
        public static IDispatcherTimer AppTimer;
        public static bool IsChronoRunning = false;
        public static bool IsCntDwnRunning = false;
        public static TimeSpan ChronoTimer;
        public static TimeSpan CountdownTimer;

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnStart()
        {
            AppTimer = Application.Current.Dispatcher.CreateTimer();
            AppTimer.Start();
            AppTimer.Interval = TimeSpan.FromSeconds(1);
            ChronoTimer = TimeSpan.Zero;
            AppTimer.Tick += async (s, e) =>
            {
                if (IsChronoRunning) ChronoTimer = ChronoTimer.Add(TimeSpan.FromSeconds(1));
                if (IsCntDwnRunning)
                    if (CountdownTimer > TimeSpan.Zero)
                        CountdownTimer = CountdownTimer.Add(TimeSpan.FromSeconds(-1));
                    else
                    {
                        IsCntDwnRunning = false;
                        CountdownTimer = TimeSpan.Zero;
                        await Windows[0].Page.DisplayAlertAsync("Time's up!", "The countdown has finished.", "OK");
                    }
                if (IsCntDwnRunning || IsChronoRunning)
                    Debug.WriteLine($"Chrono: {ChronoTimer}, Countdown: {CountdownTimer}");
            };
        }
    }
}