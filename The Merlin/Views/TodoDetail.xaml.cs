using System.Diagnostics;
using System.Threading.Tasks;
using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.Timer;

namespace The_Merlin.Views;

[QueryProperty(nameof(qTodoId), "todoid")]
public partial class TodoDetail : ContentPage
{
    public string qTodoId { get; set { BindingContext = App.DataManager.TodoData.GetItem(int.Parse(value)); } }
    public TodoItem myTodo;
    public TodoDetail(TodoItem _todoId)
    {
        InitializeComponent();
        if (qTodoId == null)
        {
            BindingContext = _todoId;
        }
        myTodo = (TodoItem)BindingContext;
        TimeTypePckr.SelectedIndex = myTodo.TimeMode;
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

    private void TimeTypePckr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TimeTypePckr.SelectedIndex != -1)
        {
            if (TimeTypePckr.SelectedIndex != myTodo.TimeMode)
            {
                myTodo.TimeMode = TimeTypePckr.SelectedIndex;
                App.DataManager.TodoData.UpdateItem(myTodo);
            }
            LoadTimerForThisTodo();
        }
    }

    private void LoadTimerForThisTodo()
    {
        TimerStack.Clear();
        switch (myTodo.TimeMode)
        {
            case 1: TimerStack.Add(new CountdownView(myTodo)); break;
            case 2: TimerStack.Add(new ChronoView()); break;
            case 3: TimerStack.Add(new CountdownView(myTodo)); break;
            default: break;
        }
    }
}