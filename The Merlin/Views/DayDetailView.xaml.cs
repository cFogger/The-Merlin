using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class DayDetailView : ContentPage
{
	public DayDetailView(DayDetailViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}