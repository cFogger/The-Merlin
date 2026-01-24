using The_Merlin.Data;

namespace The_Merlin;


[QueryProperty(nameof(DayOfPage), "day")]
public partial class DayView : ContentPage
{
    DateTime selectedDate;
    public string DayOfPage { set { if (!string.IsNullOrEmpty(value)) selectedDate = new DateTime(long.Parse(value.Split('/')[0])); else selectedDate = DateTime.Today; ReloadTodos(); } }

    private TodoData _todoData;
    public DayView(TodoData todoData)
	{
        _todoData = todoData;
        InitializeComponent();
		this.BindingContext = this;
        //ReloadTodos();
    }

    public void ReloadTodos()
    {
        MainStack.Children.Clear();

        this.Title = selectedDate.ToString("dd.MM.yy");

        foreach (var item in _todoData.GetUndoneItems(selectedDate))
            MainStack.Children.Add(new Views.TodoView(item));

        foreach (var item in _todoData.GetDoneItems(selectedDate))
            MainStack.Children.Add(new Views.TodoView(item));

        MainStack.Children.Add(new Views.TodoAdd(ReloadTodos));
    }
}