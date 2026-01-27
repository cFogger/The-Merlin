using The_Merlin.CustomControls;
using The_Merlin.Data;

namespace The_Merlin.Views;


[QueryProperty(nameof(DayOfPage), "day")]
public partial class DayView : ContentPage
{
    DateTime selectedDate;
    public string DayOfPage { set { if (!string.IsNullOrEmpty(value)) selectedDate = new DateTime(long.Parse(value.Split('/')[0])); else selectedDate = DateTime.Today; } }

    private TodoData _todoData;
    public DayView(TodoData todoData)
	{
        _todoData = todoData;
        InitializeComponent();
		this.BindingContext = this;
    }
}