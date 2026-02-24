using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using The_Merlin.Models;

namespace The_Merlin.CustomControls;

public partial class HabitItemPopup : Popup<HabitItem>
{
    public ObservableCollection<string> AvailableColors { get; } = new ObservableCollection<string>
    {
        "#FF7043", // Koyu Turuncu
        "#26A69A", // Teal
        "#8D6E63", // Toprak Kahverengi
        "#789262", // Zeytin Yeþili
        "#D4E157", // Parlak Sarý-Yeþil
        "#5C6BC0", // Koyu Mavi
        "#EC407A", // Canlý Pembe
        "#00B8D4", // Parlak Camgöbeði
        "#C62828", // Koyu Kýrmýzý
        "#43A047", // Koyu Yeþil
        "#FFD600", // Neon Sarý
        "#6D4C41", // Çikolata Kahverengi
        "#00ACC1", // Koyu Camgöbeði
        "#F06292", // Açýk Pembe
        "#7E57C2"  // Mor
    };

    public string SelectedColor { get; set; } // Holds the selected color

    public HabitItemPopup(HabitItem item)
    {
        InitializeComponent();
        BindingContext = this;

        // Initialize the HabitItem as the BindingContext for the popup
        HabitItem = item;
    }

    public HabitItem HabitItem { get; }

    public async void OnSaveClicked(object sender, EventArgs e)
    {
        // Assign the selected color to the HabitItem
        if (!string.IsNullOrEmpty(SelectedColor))
        {
            HabitItem.Color = SelectedColor;
        }

        await CloseAsync(HabitItem);
    }

    public async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync(null);
    }

    public void OnColorSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string color)
        {
            SelectedColor = color;
        }
    }
}