using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin;

public partial class TodoAddPage : ContentPage
{
	private DataManager _dataManager;
	public TodoAddPage(DataManager data)
	{
		InitializeComponent();
		this.BindingContext = this;
		_dataManager = data;
    }

	public string priorityValue { get; set; }

    private void Button_Clicked(object sender, EventArgs e)
    {
		TodoItem newTodo = new TodoItem()
		{
			TodoText = TitleEntry.Text,
			TodoInside = TextEditor.Text,
			IsCompleted = false,
			Status = 0,
			CreatedAt = DateTime.Now,
			AutoCreated = false,
			AssignedDate = AssignedDatePck.Date.GetValueOrDefault(DateTime.Today),
			ParentId = 0,
			RepeatSpan = TimeSpan.Zero,
			Priority = int.Parse(priorityValue)
        };
		_dataManager.TodoData.AddItem(newTodo);
    }
}