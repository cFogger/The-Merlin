using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using The_Merlin.Models;

namespace The_Merlin.CustomControls;

public partial class TimelineItemDetailPopup : Popup<TimelineItem>
{
	public TimelineItemDetailPopup(TimelineItem ti)
	{
		InitializeComponent();
        BindingContext = ti;
    }


    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        var tli = (TimelineItem)BindingContext;
        tli.Starts = tli.Starts.Date + TimeSpan.FromMinutes(StartPicker.Time.Value.TotalMinutes);
        tli.Ends = tli.Ends.Value.Date + TimeSpan.FromMinutes(EndsPicker.Time.Value.TotalMinutes);

        await CloseAsync(tli);
    }
}