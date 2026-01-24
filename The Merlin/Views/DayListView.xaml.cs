using The_Merlin.Data;

namespace The_Merlin.Views;

public partial class DayListView : ContentPage
{
	public DayListView(TodoData todoData)
	{
		InitializeComponent();

        foreach (DateTime dtti in todoData.GetAssignedDates().OrderByDescending(x => x.Ticks))
        {
            mainStack.Add(new Button
            {
                Text = dtti.ToString("dd.MM.yy") + (dtti.Date == DateTime.Today ? " (Today)" : ""),
                Command = new Command(async () =>
                {
                    if (AppShell.Current.FlyoutBehavior != FlyoutBehavior.Locked)
                        AppShell.Current.FlyoutIsPresented = false;
                    await Shell.Current.GoToAsync($"DayView?day={dtti.Ticks}");
                }),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(3),
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 200,
                FontSize = 14,
                BackgroundColor = Colors.DarkSlateGray,
                TextColor = Colors.White
            });
        }
    }
}