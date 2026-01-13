using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.Views;

public partial class TodoView : ContentView
{
    readonly DataManager dtm = App.DataManager;
    readonly TodoItem todoItem;

    public TodoView(TodoItem ti)
	{
		InitializeComponent();
        switch (ti.Priority)
        {
            case 0:
                this.BackgroundColor = Color.FromArgb("2F2F2F");
                break;
            case 1:
                this.BackgroundColor = Color.FromRgb(255, 175, 0);
                break;
            case 2:
                this.BackgroundColor = Color.FromRgb(255, 100, 0);
                break;
            case 3:
                this.BackgroundColor = Color.FromRgb(255, 0, 0);
                break;
        }
        this.BackgroundColor = ti.IsCompleted ? Colors.Green : this.BackgroundColor;
        todoItem = ti;
        TitleLabel.Text = ti.TodoText;
        TextEditor.Text = ti.TodoInside;
        if (!string.IsNullOrEmpty(ti.TodoInside))
            isItNoted.IsVisible = true;
        this.BindingContext = this;
    }

    private async void NavigateToDetail()
    {
        try
        {
            //await Shell.Current.GoToAsync($"TodoDetailView?todoid={todoItem.Id}");
            await Shell.Current.Navigation.PushAsync(new TodoDetail(todoItem));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigation to TodoDetail failed: {ex.Message}");
        }
    }

    public ICommand ClickedFromGrid => new Command(() =>
    {
        NavigateToDetail();
    });


    private void TextEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        todoItem.TodoInside = TextEditor.Text;
        dtm.TodoData.UpdateItem(todoItem);
    }
}