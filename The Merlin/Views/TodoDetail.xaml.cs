using The_Merlin.Interfaces;
using The_Merlin.Services;
using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class TodoDetail : ContentPage
{
    public TodoDetail(TodoDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        this.Appearing += (s, e) =>
        {
            App.AppTimer.Tick += vm.TodoDetailViewModel_Tick;
        };

        this.Disappearing += (s, e) =>
        {
            App.AppTimer.Tick -= vm.TodoDetailViewModel_Tick;
        };
    }
}