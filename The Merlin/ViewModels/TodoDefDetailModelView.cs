using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(tdi), "tododef")]
    public class TodoDefDetailModelView : BaseViewModel
    {
        private bool isNewItem = false;
        public TodoDefItem tdi { get { return _originalTdi; } set { _originalTdi = value; OnPropertyChanged(); } }
        private TodoDefItem _originalTdi;

        public TodoDefRepeatType[] RepeatTypes
        {
            get
            {
                return (TodoDefRepeatType[])Enum.GetValues(typeof(TodoDefRepeatType));
            }
        }

        TodoDefData _todoDefData;

        public TodoDefDetailModelView(TodoDefData todoDefData)
        {
            _todoDefData = todoDefData;
            if (tdi == null)
            {
                tdi = new TodoDefItem();
                isNewItem = true;
            }
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            Debug.WriteLine("test "+ tdi.Id);
            if (tdi.Id == 0)
                _todoDefData.AddTodoDefItem(tdi);
            else
                _todoDefData.UpdateTodoDefItem(tdi);
        });
    }
}
