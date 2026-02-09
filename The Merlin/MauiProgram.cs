using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using The_Merlin.CustomControls;
using The_Merlin.Data;
using The_Merlin.Interfaces;
using The_Merlin.Services;
using The_Merlin.ViewModels;
using The_Merlin.Views;

namespace The_Merlin
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //services
            builder.Services.AddSingleton<IMessageService, MessageService>();
            builder.Services.AddSingleton<ITimerService, TimerService>();

            //datamanagers
            builder.Services.AddSingleton<DataManager>();
            builder.Services.AddSingleton<TimelineData>();
            builder.Services.AddSingleton<DayData>();
            builder.Services.AddSingleton<TodoData>();
            builder.Services.AddSingleton<TodoDefData>();

            //single pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<TimelineLogsViewModel>();
            builder.Services.AddTransient<TimelineLogsView>();
            builder.Services.AddTransient<TodoDefListViewModel>();
            builder.Services.AddTransient<TodoDefListView>();
            builder.Services.AddTransient<DayListView>();
            builder.Services.AddTransient<DayListViewModel>();

            //reusable
            builder.Services.AddTransient<TodoDetailViewModel>();
            builder.Services.AddTransient<TodoDetail>();
            builder.Services.AddTransient<TodoDefDetailModelView>();
            builder.Services.AddTransient<TodoDefDetail>();
            builder.Services.AddTransient<DayDetailViewModel>();
            builder.Services.AddTransient<DayDetailView>();

            //customcontrols
            builder.Services.AddSingleton<FlyoutMiniTodoViewModel>();
            builder.Services.AddSingleton<FlyoutMiniTodoView>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
