using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class TodoDefListViewModel : BaseViewModel
    {
        public ObservableCollection<TodoDefItem> TodoDefItems { get; } = [];

        private TodoDefData _todoDefData;
        public TodoDefListViewModel(TodoDefData todoDefData) { 
            _todoDefData = todoDefData;
            Load();
            _todoDefData.TodoDefItemsChanged += async (s, e) =>
            {
                await _todoDefData.GetTodoDefItems(TodoDefItems);
            };
        }

        private async void Load()
        {
            await _todoDefData.GetTodoDefItems(TodoDefItems);
        }

        public ICommand AddNewTodoDefCommand => new Command(async () =>
        {
            if (AppShell.Current.FlyoutBehavior != FlyoutBehavior.Locked)
                AppShell.Current.FlyoutIsPresented = false;
            IDictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "tododef", new TodoDefItem() }
        };
            await Shell.Current.GoToAsync($"TodoDefDetailView", parameters);
        });

        public ICommand DeleteCommand => new Command<TodoDefItem>(async (tdi) =>
        {
            await _todoDefData.DeleteTodoDefItem(tdi.Id);
        });

        public ICommand NavigateToDetail => new Command<TodoDefItem>(async (tdi) =>
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "tododef", tdi }
            };

            if (AppShell.Current.FlyoutBehavior != FlyoutBehavior.Locked)
                AppShell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync($"TodoDefDetailView", parameters);
        });
    }
}
