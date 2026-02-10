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
                Fill();
                OnPropertyChanged();
                IsManualCompletion = tdi.DefaultCompletionType == TodoCompletionType.Manual;
            }
        }
        private TodoDefItem _originalTdi;

        private async void Fill()
        {
            var resolve = await _todoDefData.GetTotalDurationByTodoDefId(tdi.Id);
            TotalDuration = "Total Duration: " + resolve.ToString(@"hh\:mm\:ss");
        }

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
            tdi.RepeatType = rptType;
            await _todoDefData.AddTodoDefItem(tdi);
            await Shell.Current.GoToAsync("..");
        });
    }
}
