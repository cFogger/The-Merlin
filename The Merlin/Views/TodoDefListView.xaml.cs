using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.Views;

public partial class TodoDefListView : ContentPage
{
    public List<TodoDefItem> TodoDefItems { get => _todoDefItems; 
        set 
        { 
            _todoDefItems = value; 
            OnPropertyChanged(nameof(TodoDefItems)); 
        }
    }
    private List<TodoDefItem> _todoDefItems;

    private readonly TodoDefData _todoDefData;
    public TodoDefListView(TodoDefData todoDefData)
    {
        _todoDefData = todoDefData;
        InitializeComponent();
        this.BindingContext = this;
        TodoDefItems = _todoDefData.GetAllTodoDefItems();
        _todoDefData.TodoDefItemsChanged += (s, e) => {
            TodoDefItems = _todoDefData.GetAllTodoDefItems();
        };
    }

    public ICommand AddNewTodoDefCommand => new Command(async () =>
    {
        Debug.WriteLine("Navigating to TodoDefDetail");
        if (AppShell.Current.FlyoutBehavior != FlyoutBehavior.Locked)
            AppShell.Current.FlyoutIsPresented = false;
        IDictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "tododef", new TodoDefItem() }
        };
        await Shell.Current.GoToAsync($"TodoDefDetailView", parameters);
    });

    public ICommand DeleteCommand => new Command<TodoDefItem>((tdi) =>
    {
        _todoDefData.DeleteTodoDefItem(tdi);
    });
}