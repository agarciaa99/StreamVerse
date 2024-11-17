using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StreamVerse.Pages;
using StreamVerse.Services;
using StreamVerse.ViewModels;

namespace StreamVerse
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
                    fonts.AddFont("ubuntu-latin-400-normal.ttf", "UbuntuRegular");
                    fonts.AddFont("ubuntu-latin-700-normalttf", "UbuntuSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient(TmdbService.TmdbHttpClientName,
                httpClient => httpClient.BaseAddress =  new Uri("https://api.themoviedb.org"));

            builder.Services.AddSingleton<TmdbService>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}
