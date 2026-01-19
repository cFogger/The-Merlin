using Microsoft.Extensions.Logging;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IMessageService, MessageService>();
            builder.Services.AddSingleton<ITimerService, TimerService>();
            builder.Services.AddSingleton<DataManager>();

            builder.Services.AddTransient<TodoDetailViewModel>();
            builder.Services.AddSingleton<FlyoutMiniTodoViewModel>();

            builder.Services.AddTransient<TodoDetail>();
            builder.Services.AddSingleton<FlyoutMiniTodoView>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
