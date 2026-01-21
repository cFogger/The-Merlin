using Microsoft.Extensions.Configuration;
using The_Merlin.Data;
using The_Merlin.ViewModels;

namespace The_Merlin
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
