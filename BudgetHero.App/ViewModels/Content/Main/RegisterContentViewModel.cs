
using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Common.EntityDTOs.Account;

namespace BudgetHero.App.ViewModels.Content.Main
{
    public partial class RegisterContentViewModel : MainCarouselItemViewModelBase, IBusyHandler
    {
        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _passwordRepeated;

        [ObservableProperty]
        private string _email;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IRegisterService _registerService;
        private readonly IModalDisplayHandler _displayHandler;

        public RegisterContentViewModel() : this(App.Services.GetService<IRegisterService>()!, App.Services.GetService<IModalDisplayHandler>()!)
        {

        }

        public RegisterContentViewModel(IRegisterService registerService, IModalDisplayHandler displayHandler) : base(MainCarouselItemViewType.Register)
        {
            _registerService = registerService;
            _displayHandler = displayHandler;

            Email = string.Empty;
            Password = string.Empty;
            PasswordRepeated = string.Empty;
        }

        [RelayCommand]
        public async Task Register()
        {
            //TODO: Poprawa rzucania i obsługi wyjątków w rejestracji jak i w cały kliencie API
            await this.RunWithBusyFlagAsync(async () =>
            {
                try
                {
                    if (Password.Equals(PasswordRepeated))
                    {
                        await _registerService.RegisterAsync(new RegisterRequestModel() { Email = Email, Password = Password });
                    }
                    else
                    {
                        throw new InvalidOperationException("Nieudana próba rejestracji.");
                    }
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Nieudana próba rejestracji.");
                }
            }, new Message("Pomyślna akcja", "Rejestracja powiodła sie."));
        }

        public async override Task ResetView()
        {
            Password = string.Empty;
            PasswordRepeated = string.Empty;
            Email = string.Empty;

            await Task.CompletedTask;
        }
    }
}
