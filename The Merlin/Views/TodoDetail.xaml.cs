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
    }
}