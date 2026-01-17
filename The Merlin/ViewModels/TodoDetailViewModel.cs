using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    public class TodoDetailViewModel : BaseViewModel
    {
        private TodoItem _todo;
        public TodoItem Todo { get { return _todo; } set { _todo = value; OnPropertyChanged(); } }

        public Command StartCommand { get; }
        public Command StopCommand { get; }
        public Command DeleteCommand { get; }

        public TodoDetailViewModel(TodoItem todo)
        {
            Todo = todo;

            StartCommand = new Command(OnStart);
            StopCommand = new Command(OnStop);
            DeleteCommand = new Command(Delete);
        }

        private void OnStart() { }
        private void OnStop() { }
        private void Delete() { }

    }
}
