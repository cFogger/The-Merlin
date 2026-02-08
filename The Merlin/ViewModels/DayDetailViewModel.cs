using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(myDayItem), "dayitem")]
    public class DayDetailViewModel : BaseViewModel
    {
        public DayItem myDayItem { get { return _myDayItem; } set { 
                _myDayItem = value; 
                OnPropertyChanged();

                onTodoDefsChanged(this, EventArgs.Empty);
                onTodoItemsChanged(this, EventArgs.Empty);
            }
        }
        private DayItem _myDayItem;

        public ObservableCollection<TodoItem> TodoItems { get; } = [];
        public ObservableCollection<TodoDefItem> TodoDefs { get; } = [];


        private TodoData _todoData;
        private DayData _dayData;
        private TodoDefData _todoDefData;

        public DayDetailViewModel(DayData dayData, TodoDefData todoDefData, TodoData todoData) 
        { 
            _dayData = dayData;
            _todoDefData = todoDefData;
            _todoData = todoData;

            _todoDefData.TodoDefItemsChanged += onTodoDefsChanged;
            _todoData.TodoItemCollectionChanged += onTodoItemsChanged;
        }

        public void onTodoDefsChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var items = await _todoDefData.GetAllTodoDefItems();
                TodoDefs.Clear();
                foreach (var item in items)
                    TodoDefs.Add(item);
            });
        }

        public void onTodoItemsChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _todoData.GetTodos(TodoItems, myDayItem.Date);
            });
        }

        public ICommand TodoAddCommand => new Command(() => { if (SelectedTodoDef == null) return; 
            SelectedTodoDef.CreateTodoItem(_todoData, myDayItem.Date.Date); });

        public TodoDefItem SelectedTodoDef { get; set; }

        public ICommand NavigateToTodoDefListView => new Command(async () =>
        {
            await Shell.Current.GoToAsync("TodoDefListView");
        });
    }
}
