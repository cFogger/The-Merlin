using Microsoft.Maui.Controls;
using The_Merlin.Data;
using The_Merlin.Services;

namespace The_Merlin
{
    public partial class App : Application
    {
        public static IDispatcherTimer AppTimer { get; private set; }

        public App()
        {
            InitializeComponent();
            App.AppTimer = Application.Current.Dispatcher.CreateTimer();
            App.AppTimer.Interval = TimeSpan.FromSeconds(1);
            App.AppTimer.Start();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}