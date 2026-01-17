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

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnStart()
        {

        }
    }
}