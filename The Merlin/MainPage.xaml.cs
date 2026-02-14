using Microsoft.Extensions.Configuration;
using The_Merlin.Data;
using The_Merlin.ViewModels;

namespace The_Merlin
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel _vm;
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _vm = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing(); _vm.Load(); _vm.myDispatcher.Tick += InvalidateMethod;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing(); _vm.myDispatcher.Tick -= InvalidateMethod;
        }

        private void InvalidateMethod(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(graphicsView.Invalidate);
        }
    }
}
