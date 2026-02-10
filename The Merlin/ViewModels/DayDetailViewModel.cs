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
        public List<TodoDefItem> TodoDefs;


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

        public async void onTodoDefsChanged(object? sender, EventArgs e)
        {
            TodoDefs = await _todoDefData.GetAllTodoDefItems();
            DefSearchText = string.Empty;
        }

        public void onTodoItemsChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _todoData.GetTodos(TodoItems, myDayItem.Date);
            });
        }

        public ICommand AddQuickTodoDefCommand => new Command(async () =>
        {
            string temptext = DefSearchText;
            if (!string.IsNullOrEmpty(DefSearchText))
                await _todoDefData.AddTodoDefItem(new TodoDefItem
                {
                    TodoDefText = DefSearchText,
                    RepeatType = TodoDefRepeatType.None,
                    DefaultCompletionType = TodoCompletionType.Manual,
                    DefaultDuration = TimeSpan.FromMinutes(25),
                });
            DefSearchText = string.Empty;
            DefSearchText = temptext;
        });

        public ICommand TodoAddCommand => new Command<TodoDefItem>((tdi) => { if (tdi == null) return; tdi.CreateTodoItem(_todoData, _myDayItem.Date.Date); });

        public ICommand DeleteCommand => new Command(async () => { await _dayData.DeleteItem(myDayItem.Id); await Shell.Current.GoToAsync(".."); });

        public ICommand NavigateToTodoDefListView => new Command(async () =>
        {
            await Shell.Current.GoToAsync("TodoDefListView");
        });

        public ObservableCollection<TodoDefItem> FilteredTodoDefs { get; } = [];

        private string _defSearchText;
        public string DefSearchText
        {
            get { return _defSearchText; }
            set
            {
                _defSearchText = value;
                FilteredTodoDefs.Clear();
                if (_defSearchText != string.Empty)
                    foreach (var item in TodoDefs.Where(x => x.TodoDefText.Contains(_defSearchText, StringComparison.OrdinalIgnoreCase)))
                        FilteredTodoDefs.Add(item);
                else
                    foreach (var item in TodoDefs)
                        FilteredTodoDefs.Add(item);

                OnPropertyChanged();
            }
        }
    }
}
