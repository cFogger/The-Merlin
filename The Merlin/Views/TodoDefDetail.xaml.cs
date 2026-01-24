using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class TodoDefDetail : ContentPage
{
	public TodoDefDetail(TodoDefDetailModelView vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
    }
}