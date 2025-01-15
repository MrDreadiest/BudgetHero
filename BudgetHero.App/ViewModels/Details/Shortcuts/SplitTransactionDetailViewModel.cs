using BudgetHero.App.Factories.Confirmations;
using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Transactions;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Details.Shortcuts
{
    public partial class SplitTransactionDetailViewModel : ViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _modalDisplayHandler;

        public DropdownTransactionCategoryContentViewModel DropdownTransactionCategoryVM { get; }

        [ObservableProperty]
        private CalculatorPopupContentViewModel _calculatorPopupContentVM;

        [ObservableProperty]
        private Transaction _temporaryTransaction;

        [ObservableProperty]
        private ObservableCollection<TransactionListItem> _separatedTransaction;

        [ObservableProperty]
        private decimal _amountToSeparate;

        [ObservableProperty]
        private DateTime _selectedDate;

        [ObservableProperty]
        private TimeSpan _selectedTime;

        private bool _isNavigatedTo;

        private readonly IModalDisplayHandler _modalDisplayHandler;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;

        public SplitTransactionDetailViewModel(IModalDisplayHandler modalDisplayHandler, IBudgetService budgetService, IUserService userService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
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

            SeparatedTransaction = new();

            CalculatorPopupContentVM = new CalculatorPopupContentViewModel();
            CalculatorPopupContentVM.CalculationCompleted += CalculatorPopupContentVM_CalculationCompleted;

            DropdownTransactionCategoryVM = new DropdownTransactionCategoryContentViewModel(false, true);
            DropdownTransactionCategoryVM.SelectedTransactionCategoryChanged += DropdownTransactionCategoryVM_SelectedTransactionCategoryChanged;

            SelectedDate = TemporaryTransaction.Date;
            SelectedTime = TemporaryTransaction.Date.TimeOfDay;

            Title = Resources.Languages.AppResource.Shortcut_SplitTransactionDetailView_Title;
        }

        [RelayCommand]
        private void NavigatedTo() => _isNavigatedTo = true;

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
        private async Task DropRest()
        {
            TemporaryTransaction.TotalAmount = AmountToSeparate;
        }

        [RelayCommand]
        private async Task SaveTransactions()
        {
            var confirmation = ConfirmationFactory.CreateAddConfirmation<Transaction>(SeparatedTransaction.Count > 1);
            await this.RunWithBusyFlagAndConfirmationAsync(async () =>
            {
                ValidationResult validation = CanSave();

                if (validation.IsValid)
                {
                    if (!await CreateTransactions())
                    {
                        //TODO: Zasoby + poporawa obsługi błędów
                        throw new InvalidOperationException("Nie udało się dodać transakcji.");
                    }
                    else
                    {
                        ResetView();
                    }
                }
                else
                {
                    throw new InvalidOperationException(validation.ErrorMessage);
                }

            }, confirmation);
        }

        [RelayCommand]
        private async Task SeparateTransaction()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                ValidationResult validation = CanSeparate();

                if (validation.IsValid)
                {
                    SeparatedTransaction.Add(new TransactionListItem(TemporaryTransaction, DropdownTransactionCategoryVM.SelectedCategories.Last()));

                    AmountToSeparate -= TemporaryTransaction.TotalAmount;

                    TemporaryTransaction = new Transaction()
                    {
                        Id = string.Empty,
                        TransactionCategoryId = string.Empty,
                        BudgetId = _budgetService.CurrentBudget.Id,
                        CreatorId = _userService.CurrentUser.Id
                    };

                    DropdownTransactionCategoryVM.Reset().FireAndForgetSafeAsync();
                }
                else
                {
                    throw new InvalidOperationException(validation.ErrorMessage);
                }
                await Task.CompletedTask;
            });
        }

        [RelayCommand]
        private async Task UndoSeparation(TransactionListItem transaction)
        {
            var confirmation = ConfirmationFactory.CreateDeleteConfirmation<Transaction>();
            await this.RunWithBusyFlagAndConfirmationAsync(async () =>
            {
                decimal amountToAdd = transaction.Transaction.TotalAmount;
                AmountToSeparate += amountToAdd;
                SeparatedTransaction.Remove(transaction);
            }, confirmation);
        }

        private async Task<bool> CreateTransactions()
        {
            var result = await _transactionService.CreateTransactionsAsync(_budgetService.CurrentBudget, SeparatedTransaction.Select(t => t.Transaction).ToList());

            if (result)
            {
                ResetView();
            }
            return result;
        }

        private ValidationResult CanSeparate()
        {
            //TODO: Zasoby
            if (TemporaryTransaction.TotalAmount > AmountToSeparate)
                return new ValidationResult(false, "Kwota do wydzielenia przewyższa pozostałą.");

            return TemporaryTransaction.ToCreateRequest().IsRequestValid();
        }

        private ValidationResult CanSave()
        {
            //TODO: Zasoby
            if (SeparatedTransaction.Count == 0)
                return new ValidationResult(false, "Nie wydzielono żadnej transakcji.");

            if (AmountToSeparate > 0)
                return new ValidationResult(false, "Kwota całkowita do podziału nie została wydzielona do końca.");

            return new ValidationResult(true, string.Empty);
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

            SeparatedTransaction.Clear();

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

        private void CalculatorPopupContentVM_CalculationCompleted(object? sender, decimal value)
        {
            TemporaryTransaction.TotalAmount = value;
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
