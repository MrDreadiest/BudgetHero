
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Common.EntityDTOs.Account;

namespace BudgetHero.App.ViewModels.Content.Main
{
    public partial class LoginContentViewModel : MainCarouselItemViewModelBase, IBusyHandler
    {
        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private bool _isRemembered;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;



        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IBudgetService _budgetService;
        private readonly IModalDisplayHandler _displayHandler;

        public LoginContentViewModel(IAuthenticationService authenticationService, IUserService userService, IAppSettingsService appSettingsService, IBudgetService budgetService, IModalDisplayHandler displayHandler) : base(MainCarouselItemViewType.Login)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _appSettingsService = appSettingsService;
            _budgetService = budgetService;
            _displayHandler = displayHandler;

            Password = string.Empty;
            Email = string.Empty;
            IsRemembered = false;
        }

        public LoginContentViewModel() : this(
            App.Services.GetService<IAuthenticationService>()!,
            App.Services.GetService<IUserService>()!,
            App.Services.GetService<IAppSettingsService>()!,
            App.Services.GetService<IBudgetService>()!,
            App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        [RelayCommand]
        public async Task Login()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                var result = await _authenticationService.LoginAsync(new LoginRequestModel() { Email = Email, Password = Password });

                if (result)
                {
                    if (IsRemembered)
                    {
                        _appSettingsService.SetRememberedUserEmailAsync(Email).FireAndForgetSafeAsync();
                        _appSettingsService.SetRememberedUserPasswordAsync(Password).FireAndForgetSafeAsync();
                    }

                    if (_userService.CurrentUser.IsAccountSetup)
                    {
                        if (_budgetService.CurrentBudget.IsNullOrEmpty())
                        {
                            //await Shell.Current.GoToAsync($"//{nameof(BudgetsPageAndroidView)}");
                        }
                        else
                        {
                            //await Shell.Current.GoToAsync($"//{nameof(DashboardPageAndroidView)}");
                        }
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(FirstSetupView)}");
                    }
                }
            });
        }

        public async override Task ResetViewAsync()
        {
            await this.RunWithBusyFlagAsync(async () => { await CheckIsRememberMeActive(); });

            await Task.CompletedTask;
        }

        private async Task CheckIsRememberMeActive()
        {
            IsRemembered = await _appSettingsService.GetIsRememberedSwitchOnAsync();

            if (IsRemembered)
            {
                string rememberedPassword = await _appSettingsService.GetRememberedUserPasswordAsync();
                string rememberedEmail = await _appSettingsService.GetRememberedUserEmailAsync();

                if (!string.IsNullOrEmpty(rememberedEmail) && !string.IsNullOrEmpty(rememberedPassword))
                {
                    Password = rememberedPassword;
                    Email = rememberedEmail;
                }
            }
        }

        partial void OnIsRememberedChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                _appSettingsService.SetIsRememberedSwitchOnAsync(newValue).FireAndForgetSafeAsync();
            }
        }
    }
}
