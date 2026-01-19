using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class TodoAdd : ContentView
{
    readonly Action reloadDatas;

    public ICommand AddCommand => new Command(() =>
    {
        Application.Current.Handler.MauiContext.Services.GetService<DataManager>().TodoData.AddItem(new TodoItem()
        {
            TodoText = TitleEntry.Text,
            CreatedAt = DateTime.Now,
            IsCompleted = false,
            AssignedDate = AssignedDate.Date.GetValueOrDefault(DateTime.Today)
        });
        reloadDatas();
    });

    public TodoAdd(Action _reloadDatas)
    {
        InitializeComponent();
        this.BindingContext = this;
        reloadDatas = _reloadDatas;
        AssignedDate.Date = DateTime.Today.Date;
    }
}