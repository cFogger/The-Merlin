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

            Routing.RegisterRoute("DayView", typeof(DayDetailView));
            Routing.RegisterRoute("TodoDetailView", typeof(TodoDetail));
            Routing.RegisterRoute("TodoDefDetailView", typeof(TodoDefDetail));
        }
    }
}
