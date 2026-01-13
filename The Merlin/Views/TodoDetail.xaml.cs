using System.Diagnostics;
using System.Threading.Tasks;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.Views;

[QueryProperty(nameof(qTodoId), "todoid")]
public partial class TodoDetail : ContentPage
{
	public string qTodoId { get; set { BindingContext = App.DataManager.TodoData.GetItem(int.Parse(value)); } }

    public TodoDetail(TodoItem _todoId)
	{
		InitializeComponent();
        if (qTodoId == null)
        {
            BindingContext = _todoId;
        }
	}

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        App.DataManager.TodoData.DeleteItem((TodoItem)this.BindingContext);
        await Shell.Current.Navigation.PopAsync();
    }

    private void ToggleButton1_Clicked(object sender, EventArgs e)
    {
        TodoItem ti = (TodoItem)this.BindingContext;
        ti.IsCompleted = !ti.IsCompleted;
        ToggleButton1.BackgroundColor = ti.IsCompleted ? Colors.Green : Color.FromArgb("2F2F2F");
        App.DataManager.TodoData.UpdateItem(ti);
    }
}