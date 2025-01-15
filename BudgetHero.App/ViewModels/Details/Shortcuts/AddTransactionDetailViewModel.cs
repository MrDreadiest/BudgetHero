using BudgetHero.App.Factories.Confirmations;
using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Details.Shortcuts
{
    public partial class AddTransactionDetailViewModel : ViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _modalDisplayHandler;

        public DropdownTransactionCategoryContentViewModel DropdownTransactionCategoryVM { get; }

        [ObservableProperty]
        private Transaction _temporaryTransaction;

        [ObservableProperty]
        private DateTime _selectedDate;

        [ObservableProperty]
        private TimeSpan _selectedTime;

        private bool _isNavigatedTo;

        private readonly IModalDisplayHandler _modalDisplayHandler;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;

        public AddTransactionDetailViewModel(IModalDisplayHandler modalDisplayHandler, IBudgetService budgetService, IUserService userService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
        {
            _modalDisplayHandler = modalDisplayHandler;
            _budgetService = budgetService;
            _userService = userService;
            _transactionService = transactionService;

            TemporaryTransaction = new Transaction()
            {
                Id = string.Empty,
                TransactionCategoryId = string.Empty,
                BudgetId = string.Empty,
                CreatorId = string.Empty
            };

            DropdownTransactionCategoryVM = new DropdownTransactionCategoryContentViewModel(false, true);
            DropdownTransactionCategoryVM.SelectedTransactionCategoryChanged += DropdownTransactionCategoryVM_SelectedTransactionCategoryChanged;

            SelectedDate = TemporaryTransaction.Date;
            SelectedTime = TemporaryTransaction.Date.TimeOfDay;

            Title = Resources.Languages.AppResource.Shortcut_AddTransactionDetailView_Title;
        }

        [RelayCommand]
        private void NavigatedTo()
        {
            _isNavigatedTo = true;
            DropdownTransactionCategoryVM.Hide();
        }

        [RelayCommand]
        private void NavigatedFrom() => _isNavigatedTo = false;

        [RelayCommand]
        private async Task Appearing()
        {
            if (!_isNavigatedTo)
            {
                await Refresh();
            }
        }

        [RelayCommand]
        private async Task ProcessAction()
        {
            var confirmation = ConfirmationFactory.CreateAddConfirmation<Transaction>();
            await this.RunWithBusyFlagAndConfirmationAsync(async () =>
            {
                if (!await CreateTransaction())
                {
                    //TODO: Zasoby + poporawa obsługi błędów
                    throw new InvalidOperationException("Nie udało się dodać transakcji.");
                }
            }, confirmation);
        }

        private async Task<bool> CreateTransaction()
        {
            ValidationResult validation = TemporaryTransaction.ToCreateRequest().IsRequestValid();

            if (validation.IsValid)
            {
                var result = await _transactionService.CreateTransactionAsync(_budgetService.CurrentBudget, TemporaryTransaction);

                if (result)
                {
                    ResetView();
                    return true;
                }
            }
            return false;
        }

        private async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await DropdownTransactionCategoryVM.Refresh();
                ResetView();
            });
        }

        private void ResetView()
        {
            TemporaryTransaction = new Transaction()
            {
                Id = string.Empty,
                TransactionCategoryId = string.Empty,
                BudgetId = _budgetService.CurrentBudget.Id,
                CreatorId = _userService.CurrentUser.Id
            };

            SelectedDate = TemporaryTransaction.Date;
            SelectedTime = TemporaryTransaction.Date.TimeOfDay;

            DropdownTransactionCategoryVM.Reset().FireAndForgetSafeAsync();
        }

        partial void OnSelectedDateChanged(DateTime oldValue, DateTime newValue)
        {
            TemporaryTransaction.Date = new DateTime(newValue.Year, newValue.Month, newValue.Day, newValue.Hour, newValue.Minute, newValue.Second);
        }

        partial void OnSelectedTimeChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            TemporaryTransaction.Date = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, newValue.Hours, newValue.Minutes, newValue.Seconds);
        }

        private void DropdownTransactionCategoryVM_SelectedTransactionCategoryChanged(object? sender, List<Models.TransactionCategory> e)
        {
            if (e.Count != 0)
            {
                TemporaryTransaction.Name = e.Last().Name;
                TemporaryTransaction.TransactionCategoryId = e.Last().Id;
            }
        }
    }
}
