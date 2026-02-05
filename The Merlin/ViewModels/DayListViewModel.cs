using System.Collections.ObjectModel;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class DayListViewModel : BaseViewModel
    {
        public ObservableCollection<DayItem> dayItems { get; } = [];

        private readonly DayData _dayData;
        public DayListViewModel(DayData dayData, TodoData todoData) 
        { 
            _dayData = dayData;
            todoData.GetAssignedDates();
            _dayData.ItemChanged += onDayItemsChanged;
            onDayItemsChanged(this, EventArgs.Empty);
            SelectedDate = DateTime.Today;
        }

        private void onDayItemsChanged(object? sender, EventArgs e)
        {
            _dayData.GetAllDays(dayItems);
        }

        public ICommand GoToDetail => new Command<DayItem>(async (di) =>
        {
            var parameters = new Dictionary<string, object>()
            {
                { "dayitem", di }
            };
            await Shell.Current.GoToAsync("DayView", parameters);
        });

        private DateTime _selectedDate;
        public DateTime SelectedDate { get { return _selectedDate; } set { _selectedDate = value; OnPropertyChanged(); _dayData.AddItem(new DayItem() { Date = value, DayType = DayType.HomeDay }); } }
    }
}
