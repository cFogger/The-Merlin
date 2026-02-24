using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.CustomControls;
using CommunityToolkit.Maui.Extensions;
using System.Diagnostics;

namespace The_Merlin.ViewModels
{
    public class HabitItemsViewModel : BaseViewModel
    {
        public ObservableCollection<HabitItem> HabitItems { get; } = new();

        private readonly HabitData _habitData;

        public HabitItemsViewModel(HabitData habitData)
        {
            _habitData = habitData;
            Load();
            _habitData.HabitItemsChanged += async (s, e) =>
            {
                await _habitData.GetHabitItems(HabitItems);
            };
        }

        private async void Load()
        {
            await _habitData.GetHabitItems(HabitItems);
        }

        public ICommand AddHabitCommand => new Command(async () =>
        {
            var popup = new HabitItemPopup(new HabitItem());
            var result = await Shell.Current.CurrentPage.ShowPopupAsync<HabitItem>(popup) as HabitItem;
            if (result != null)
            {
                Debug.WriteLine("is triggered");
                await _habitData.AddHabitItem(result);
            }
        });

        public ICommand EditHabitCommand => new Command<HabitItem>(async (habit) =>
        {
            var result = await Shell.Current.CurrentPage.ShowPopupAsync<HabitItem>(new HabitItemPopup(habit)) as HabitItem;
            if (result != null)
            {
                await _habitData.AddHabitItem(result); // Assuming AddHabitItem handles updates as well
            }
        });

        public ICommand DeleteHabitCommand => new Command<HabitItem>(async (habit) =>
        {
            await _habitData.DeleteHabitItem(habit.Id);
        });
    }
}
