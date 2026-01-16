using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;
using System.Diagnostics;
using The_Merlin.Data;
using The_Merlin.Timer;

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


        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnStart()
        {
            AppTimer = Application.Current.Dispatcher.CreateTimer();
            AppTimer.Interval = TimeSpan.FromSeconds(1);
            AppTimer.Start();
            AppTimer.Tick += (s, e) =>
            {
                TimerService.Tick();
            };
        }
    }
}