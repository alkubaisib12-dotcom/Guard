using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace GuardPrototype
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Optional: Register services here if needed
            // builder.Services.AddSingleton<SomeService>();

            return builder.Build();
        }
    }
}
