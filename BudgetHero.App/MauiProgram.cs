using BudgetHero.App.Services;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace BudgetHero.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            #region Services

            builder.Services.AddSingleton<IModalDisplayHandler, ModalDisplayHandler>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IBudgetService, BudgetService>();
            builder.Services.AddSingleton<ITransactionCategoryService, TransactionCategoryService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();

            builder.Services.AddTransient<IAppSettingsService, AppSettingsService>();
            builder.Services.AddTransient<IConfigurationService, ConfigurationService>();
            builder.Services.AddTransient<IApiClient, ApiClient>();
            builder.Services.AddTransient<IRegisterService, RegisterService>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<ITokenService, TokenService>();

            #endregion

            #region ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<FirstSetupViewModel>();

            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<BudgetsViewModel>();
            builder.Services.AddSingleton<ReportsViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<TransactionsViewModel>();
            #endregion

            return builder.Build();
        }
    }
}
