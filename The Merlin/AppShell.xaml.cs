using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.Services;
using The_Merlin.ViewModels;
using The_Merlin.Views;

namespace The_Merlin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("DayView", typeof(DayView));
            Routing.RegisterRoute("TodoDetailView", typeof(TodoDetail));

            //TodoDates MenuItems
            foreach (DateTime dtti in Application.Current.Handler.GetService<DataManager>().TodoData.GetAssignedDates().OrderByDescending(x=>x.Ticks))
            {
                Items.Add(new MenuItem
                {
                    Text = dtti.ToString("dd.MM.yy") + (dtti.Date == DateTime.Today ? " (Today)" : "" ),
                    Command = new Command(async () =>
                    {
                        if (AppShell.Current.FlyoutBehavior != FlyoutBehavior.Locked)
                            AppShell.Current.FlyoutIsPresented = false;
                        await Shell.Current.GoToAsync($"DayView?day={dtti.Ticks}");
                    })
                });
            }
        }
    }
}
