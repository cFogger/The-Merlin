using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class TodoDefListView : ContentPage
{
    public TodoDefListView(TodoDefListViewModel vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }
}