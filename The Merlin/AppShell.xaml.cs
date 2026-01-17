using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.Services;
using The_Merlin.Views;

namespace The_Merlin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("DayView", typeof(DayView));
            Routing.RegisterRoute("TodoAddView", typeof(TodoAddPage));
            Routing.RegisterRoute("TodoDetailView", typeof(TodoDetail));

            Items.Add(new MenuItem
            {
                Text = "TodoAdd",
                Command = new Command(async () =>
                {
                    if (AppShell.Current.FlyoutBehavior != FlyoutBehavior.Locked)
                        AppShell.Current.FlyoutIsPresented = false;
                    await Shell.Current.GoToAsync("TodoAddView");
                })
            });

            //TodoDates MenuItems
            foreach (DateTime dtti in App.DataManager.TodoData.GetAssignedDates().OrderByDescending(x=>x.Ticks))
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
