using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Services;
using ArbitrageAgent.ViewModel;
using ArbitrageAgent.ViewModel.Models;
using Microsoft.Extensions.Logging;

namespace ArbitrageAgent
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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "arbitrageAgent.db3");

            // Dependency injection
            builder.Services.AddSingleton(new AppDbContext(dbPath));
            builder.Services.AddSingleton<IDataRepository, DataRepository>();
            builder.Services.AddSingleton<HeartbeatService>();
            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<MaxWeightedRouteService>();
            builder.Services.AddSingleton<AssetLinkGraphService>();
            builder.Services.AddSingleton<WeightedRouteViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            // Force creation and async load of SettingsVM
            var settingsService = app.Services.GetRequiredService<SettingsViewModel>();

            // Optionally trigger async initialization
            _ = Task.Run(async () => await settingsService.InitializeAsync());

            return app;
        }
    }
}
