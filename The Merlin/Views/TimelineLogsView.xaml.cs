using The_Merlin.ViewModels;

namespace The_Merlin.Views;

public partial class TimelineLogsView : ContentPage
{
	public TimelineLogsView(TimelineLogsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}