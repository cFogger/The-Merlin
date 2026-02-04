using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(tdi), "tododef")]
    public class TodoDefDetailModelView : BaseViewModel
    {
        public TodoDefItem tdi
        {
            get { return _originalTdi; }
            set
            {
                _originalTdi = value;
                rptType = value.RepeatType;
                OnPropertyChanged();
                TotalDuration = "Total Duration: " + _todoDefData.GetTotalDurationByTodoDefId(tdi.Id).ToString(@"hh\:mm\:ss");
                IsManualCompletion = tdi.DefaultCompletionType == TodoCompletionType.Manual;
            }
        }
        private TodoDefItem _originalTdi;

        public TodoDefRepeatType rptType { get { return _rptType; } 
            set 
            { 
                _rptType = value; 
                OnPropertyChanged(); 
                IsCustomRpt = (_rptType == TodoDefRepeatType.Custom);
            }
        }
        private TodoDefRepeatType _rptType;

        public bool IsCustomRpt { get { return _isCustomRpt; } set { _isCustomRpt = value; OnPropertyChanged(); } }
        private bool _isCustomRpt;

        private bool _isManualCompletion;
        public bool IsManualCompletion
        {
            get { return _isManualCompletion; }
            set
            {
                _isManualCompletion = value;
                OnPropertyChanged("IsManualCompletion");
                tdi.DefaultCompletionType = ((!IsManualCompletion) ? TodoCompletionType.DurationBased : TodoCompletionType.Manual);
            }
        }

        public string TotalDuration { get { return _totalDuration; } set { _totalDuration = value; OnPropertyChanged(); } }
        private string _totalDuration;

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
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            Debug.WriteLine("test "+ tdi.Id);
            tdi.RepeatType = rptType;
            if (tdi.Id == 0)
                _todoDefData.AddTodoDefItem(tdi);
            else
                _todoDefData.UpdateTodoDefItem(tdi);
            await Shell.Current.GoToAsync("..");
        });


    }
}
