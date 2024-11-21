using BudgetHero.App.Models;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.FirstSetup;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels
{
    public partial class FirstSetupViewModel : ViewModelBase, IBusyHandler
    {
        private bool _isNavigatedTo;
        private bool _dataLoaded;

        public List<FirstSetupCarouselItemViewModelBase> FirstSetupCarouselVMs { get; }

        public SegmentedControlViewModel SegmentedControlVM { get; }
        public IconSelectContentViewModel IconSelectVM { get; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        [ObservableProperty]
        private int _carouselPosition;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public FirstSetupViewModel(IModalDisplayHandler displayHandler, IAuthenticationService authenticationService, IAppSettingsService appSettingsService, IUserService userService, IBudgetService budgetService, ITransactionCategoryService transactionCategoryService)
        {
            _displayHandler = displayHandler;
            _authenticationService = authenticationService;
            _appSettingsService = appSettingsService;
            _userService = userService;
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;

            IconSelectVM = new IconSelectContentViewModel();

            FirstSetupCarouselVMs = new List<FirstSetupCarouselItemViewModelBase>()
            {
                new UserContentViewModel(),
                new BudgetContentViewModel(IconSelectVM),
                new CategoriesContentViewModel(IconSelectVM),
            };

            SegmentedControlVM = new SegmentedControlViewModel(FirstSetupCarouselVMs.Select(item => new SegmentedControlItem(item.Title)).ToList(), false);
        }

        [RelayCommand]
        public async Task Next()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                var currentVM = FirstSetupCarouselVMs[CarouselPosition];
                if (currentVM.CanGoNext())
                {
                    if (!IncrementCarouselPosition())
                    {
                        //TODO: zasoby
                        var isConfirmed = await AskForConfirmation("Czy na pewno chcesz zakończyć konfigurację konta?");
                        if (!isConfirmed)
                            return;

                        bool result = await FinalizeUserAccountSetup();
                        if (result)
                        {
                            await Shell.Current.GoToAsync($"//{nameof(BudgetsView)}");
                        }
                    }
                }
                else
                {
                    //TODO: zasoby
                    throw new InvalidOperationException("Błędny formularz.");
                }
            });
        }

        [RelayCommand]
        public async Task Back()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                if (!DecrementCarouselPosition())
                {
                    //TODO: zasoby
                    var isConfirmed = await AskForConfirmation("Czy na pewno chcesz przerwać konfigurację konta?");
                    if (!isConfirmed)
                        return;

                    await _authenticationService.LogoutAsync();
                    await Shell.Current.GoToAsync($"//{nameof(MainView)}");
                }
            });
        }

        [RelayCommand]
        private void NavigatedTo() => _isNavigatedTo = true;

        [RelayCommand]
        private void NavigatedFrom() => _isNavigatedTo = false;

        [RelayCommand]
        private async Task Appearing()
        {
            if (!_dataLoaded)
            {
                await InitData();
                _dataLoaded = true;
                await Refresh();
            }
            // This means we are being navigated to
            else if (!_isNavigatedTo)
            {
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            IconSelectVM.ResetView();

            foreach (var item in FirstSetupCarouselVMs)
            {
                await item.ResetViewAsync();
            }
        }

        private async Task InitData()
        {
            await Task.CompletedTask;
        }

        private bool IncrementCarouselPosition()
        {

            if (CarouselPosition < FirstSetupCarouselVMs.Count - 1)
            {
                var oldValue = CarouselPosition;
                var newValue = ++CarouselPosition;

                NotifyVMs(oldValue, newValue);

                return true;
            }
            return false;
        }

        private bool DecrementCarouselPosition()
        {
            if (CarouselPosition > 0)
            {
                var oldValue = CarouselPosition;
                var newValue = --CarouselPosition;

                NotifyVMs(oldValue, newValue);

                return true;
            }
            return false;
        }

        private void NotifyVMs(int oldValue, int newValue)
        {
            SegmentedControlVM.Select(newValue);
            if (oldValue != newValue)
            {
                FirstSetupCarouselVMs[oldValue].OnDisappearingAsync().FireAndForgetSafeAsync();
            }
            FirstSetupCarouselVMs[newValue].OnAppearingAsync().FireAndForgetSafeAsync();
        }

        private async Task<bool> AskForConfirmation(string message)
        {
            return await Shell.Current.DisplayAlert("Potwierdzenie", message, "Tak", "Nie");
        }

        private async Task<bool> FinalizeUserAccountSetup()
        {
            try
            {
                var userVM = GetAccountSetupViewModel<UserContentViewModel>();
                var budgetVM = GetAccountSetupViewModel<BudgetContentViewModel>();
                var categoryVM = GetAccountSetupViewModel<CategoriesContentViewModel>();

                if (userVM == null || budgetVM == null || categoryVM == null)
                    return false;

                bool userResult = await UpdateUserAsync(userVM);
                if (!userResult)
                    return false;

                bool budgetResult = await CreateBudgetAsync(budgetVM);
                if (!budgetResult)
                    return false;

                var createdBudget = _budgetService.OwnBudgets.FirstOrDefault();
                if (createdBudget == null)
                    return false;

                bool categoriesResult = await CreateTransactionCategoriesAsync(createdBudget, categoryVM);
                return categoriesResult;
            }
            catch (Exception ex)
            {
                //TODO: Handle exception (maybe log it)
                return false;
            }
        }

        private async Task<bool> UpdateUserAsync(UserContentViewModel userVM)
        {
            return await _userService.UpdateUserAsync(userVM.TemporaryUser);
        }

        private async Task<bool> CreateBudgetAsync(BudgetContentViewModel budgetVM)
        {
            return await _budgetService.CreateBudgetAsync(budgetVM.TemporaryBudget);
        }

        private async Task<bool> CreateTransactionCategoriesAsync(Budget createdBudget, CategoriesContentViewModel categoryVM)
        {
            return await _transactionCategoryService.CreateTransactionCategoriesAsync(
                createdBudget, categoryVM.TransactionCategories.ToList()
            );
        }

        private T? GetAccountSetupViewModel<T>() where T : FirstSetupCarouselItemViewModelBase
        {
            return FirstSetupCarouselVMs.OfType<T>().FirstOrDefault();
        }
    }
}
