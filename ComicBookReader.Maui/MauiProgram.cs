using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using ComicBookReader.Maui.Views;
using ComicBookReader.Maui.ViewModels;
using ComicBookReader.UseCases.Interfaces;
using ComicBookReader.UseCases;
using ComicBookReader.UseCases.PluginInterfaces;
using ComicBookReader.Plugins.DataStore.InMemory;

namespace ComicBookReader.Maui
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IComicFolderRepository, ComicFolderInMemoryRepository>();
            builder.Services.AddSingleton<IAddComicFolderUseCase, AddComicFolderUseCase>();
            builder.Services.AddSingleton<IViewIssueUseCase, ViewIssueUseCase>();

            builder.Services.AddSingleton<IssuesViewModel>();
            builder.Services.AddSingleton<IssueViewModel>();

            builder.Services.AddSingleton<IssuesPage>();
            builder.Services.AddSingleton<ReadIssuePage>();

            return builder.Build();
        }
    }
}
