using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;
using The_Merlin.Models;

namespace The_Merlin.CustomControls;

public partial class TodoItemDetailPopup : Popup<TodoItem>
{
	public TodoItemDetailPopup(TodoItem todoItem)
	{
		InitializeComponent();
        BindingContext = todoItem;
	}

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync(null);
    }

    // ViewModel içindeki SaveCommand tetiklendiðinde burayý çaðýrabilirsin
    public async void TaskSaved(object sender, EventArgs e)
    {
        await CloseAsync((TodoItem)BindingContext); // Güncel nesneyi geri gönder
    }
}