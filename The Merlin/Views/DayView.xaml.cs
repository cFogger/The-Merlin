using The_Merlin.CustomControls;
using The_Merlin.Data;

namespace The_Merlin.Views;


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
            MainStack.Children.Add(new TodoView(item));

        foreach (var item in _todoData.GetDoneItems(selectedDate))
            MainStack.Children.Add(new TodoView(item));

        MainStack.Children.Add(new TodoAdd(ReloadTodos));
    }
}