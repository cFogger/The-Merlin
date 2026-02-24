using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class HabitItemsView : ContentPage
{
	public HabitItemsView(HabitItemsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}