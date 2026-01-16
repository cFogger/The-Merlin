using System.Diagnostics;
using System.Threading.Tasks;
using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.Timer;

namespace The_Merlin.Views;

public partial class TodoDetail : ContentPage
{
    public TodoItem myTodo;
    public TodoDetail(TodoItem _todoId)
    {
        myTodo = _todoId;
        InitializeComponent();
        BindingContext = myTodo;
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        App.DataManager.TodoData.DeleteItem(myTodo);
        await Shell.Current.Navigation.PopAsync();
    }

    private void ToggleButton1_Clicked(object sender, EventArgs e)
    {
        myTodo.IsCompleted = !myTodo.IsCompleted;
        ToggleButton1.BackgroundColor = myTodo.IsCompleted ? Colors.Green : Color.FromArgb("2F2F2F");
        App.DataManager.TodoData.UpdateItem(myTodo);
    }

    private void TimeTypePckr_SelectedIndexChanged(object sender, EventArgs e)
    {
        App.DataManager.TodoData.UpdateItem(myTodo);
        LoadTimerForThisTodo();
    }

    private void LoadTimerForThisTodo()
    {
        TimerStack.Clear();
        if (myTodo.TimeMode == 1)
            TimerStack.Add(new TimerView(SaveTimelineItem));
    }

    public void SaveTimelineItem(bool isEnd)
    {
        if (!isEnd)
        {
            App.DataManager.TimelineData.AddItem(new TimelineItem { Starts = DateTime.Now, TodoId = myTodo.Id});
        }
        else
        {
            App.DataManager.TimelineData.EndItem(myTodo.Id, DateTime.Now);
        }
    }

    private void ItemChanged(object sender, TextChangedEventArgs e)
    {
        App.DataManager.TodoData.UpdateItem(myTodo); 
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        App.DataManager.TodoData.UpdateItem(myTodo);
    }
}