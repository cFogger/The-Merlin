using CommunityToolkit.Maui.Views;
using The_Merlin.Models;

namespace The_Merlin.CustomControls;

public partial class TimelineItemPopup : Popup
{
	public TimelineItemPopup(TimelineItem item)
	{
		InitializeComponent();
		BindingContext = item;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.CloseAsync();
    }
}