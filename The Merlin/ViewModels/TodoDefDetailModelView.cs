using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.ViewModels
{
    [QueryProperty(nameof(tdi), "tododef")]
    public class TodoDefDetailModelView : BaseViewModel
    {

        TodoDefData _todoDefData;

        public TodoDefDetailModelView(TodoDefData todoDefData)
        {
            _todoDefData = todoDefData;
        }

        public TodoDefItem tdi
        {
            get { return _originalTdi; }
            set
            {
                _originalTdi = value;
                rptType = value.RepeatType;
                colorOpt = Color.FromArgb(value.DefaultColor);
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

        public ICommand SaveCommand => new Command(async () =>
        {
            tdi.RepeatType = rptType;
            tdi.DefaultColor = colorOpt.ToHex();
            await _todoDefData.AddTodoDefItem(tdi);
            await Shell.Current.GoToAsync("..");
        });

        //RepeatTypes
        public TodoDefRepeatType[] RepeatTypes
        {
            get
            {
                return (TodoDefRepeatType[])Enum.GetValues(typeof(TodoDefRepeatType));
            }
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

        //completion
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

        //duration
        public string TotalDuration { get { return _totalDuration; } set { _totalDuration = value; OnPropertyChanged(); } }
        private string _totalDuration;
        
        //colorpick
        public record ColorOption(string Name, string Hex);
        public List<ColorOption> usedColors { get; } = new()
            {
                new ColorOption("Lavanta", "#B39DDB"),
                new ColorOption("Nane Yeşili", "#A5D6A7"),
                new ColorOption("Okyanus Mavisi", "#81D4FA"),
                new ColorOption("Mercan", "#F48FB1"),
                new ColorOption("Turkuaz", "#80CBC4"),
                new ColorOption("Gece Mavisi", "#9FA8DA"),
                new ColorOption("Günbatımı", "#FFCC80"),
                new ColorOption("Eflatun", "#CE93D8"),
                new ColorOption("Altın Sarısı", "#FFF59D"),
                new ColorOption("Mavi Gri", "#B0BEC5"),
                new ColorOption("Lime", "#E6EE9C"),
                new ColorOption("Vişne Çürüğü", "#EF9A9A"),
                new ColorOption("Siber Mavi", "#90CAF9"),
                new ColorOption("Hardal", "#FFE082"),
                new ColorOption("Somon", "#FFAB91")
            };

        private Color _colorOpt;
        public Color colorOpt { get { return _colorOpt; } set { _colorOpt = value; OnPropertyChanged(); } }
    }
}
