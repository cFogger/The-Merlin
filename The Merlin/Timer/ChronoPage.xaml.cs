namespace The_Merlin.Timer;

public partial class ChronoPage : ContentPage
{
    public ChronoPage()
    {
        InitializeComponent();
        CountDwnButton_Clicked(this, EventArgs.Empty);
    }

    private void ChronoButton_Clicked(object sender, EventArgs e)
    {
        ContentStack.Clear();
        ContentStack.Add(new ChronoView());

        ChronoButton.IsEnabled = false;
        ChronoButton.BackgroundColor = Colors.AliceBlue;
        CountDwnButton.IsEnabled = true;
        CountDwnButton.BackgroundColor = Colors.DarkGreen;
    }

    private void CountDwnButton_Clicked(object sender, EventArgs e)
    {
        ContentStack.Clear();
        ContentStack.Add(new CountdownView());

        CountDwnButton.IsEnabled = false;
        CountDwnButton.BackgroundColor = Colors.AliceBlue;
        ChronoButton.IsEnabled = true;
        ChronoButton.BackgroundColor = Colors.DarkGreen;
    }
}