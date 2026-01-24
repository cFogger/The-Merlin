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
        _todoData.AddItem(new TodoItem()
        {
            TodoText = TitleEntry.Text,
            CreatedAt = DateTime.Now,
            IsCompleted = false,
            AssignedDate = DateTime.Today
        });
        reloadDatas();
    });

    private TodoData _todoData;
    public TodoAdd(Action _reloadDatas, TodoData todoData = null)
    {
        InitializeComponent();
        this.BindingContext = this;
        reloadDatas = _reloadDatas;
        _todoData = todoData;
    }
}