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
        private readonly TodoData _todoData;
        public DayListViewModel(DayData dayData, TodoData todoData) 
        { 
            _dayData = dayData;
            _todoData = todoData;
            Load();
            SelectedDate = DateTime.Today;
        }

        private async void Load()
        {
            _dayData.ItemChanged += onDayItemsChanged;
        }

        private async void Save()
        {
            await _dayData.SaveItem(new DayItem() { Date = _selectedDate, DayType = DayType.HomeDay });
        }

        private async void onDayItemsChanged(object? sender, EventArgs e)
        {
            await _dayData.GetAllDays(dayItems);
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
        public DateTime SelectedDate { get { return _selectedDate; } set { _selectedDate = value; OnPropertyChanged(); Save(); } }
        

    }
}
