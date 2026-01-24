using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class FlyoutMiniTodoView : ContentView
{
	public FlyoutMiniTodoView()
	{
		InitializeComponent();
        BindingContext = Application.Current.Handler.MauiContext.Services.GetService<FlyoutMiniTodoViewModel>();
    }
}