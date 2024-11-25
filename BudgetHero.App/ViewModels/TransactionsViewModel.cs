using BudgetHero.App.Factories.Confirmations;
using BudgetHero.App.Models;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Transactions;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels
{
    public partial class TransactionsViewModel : ViewModelBase, IBusyHandler
    {
        [ObservableProperty]
        private SwipeView _currentOpenSwipeView;

        [ObservableProperty]
        public ObservableCollection<TransactionListItem> _filteredTransaction = new();

        public ObservableCollection<TransactionCategory> FilteredCategories { get; set; } = new();

        public FilterTransactionsContentViewModel FilterTransactionsVM { get; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private bool _isNavigatedTo;
        private bool _dataLoaded;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        public TransactionsViewModel(IModalDisplayHandler displayHandler, IBudgetService budgetService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;
            _transactionService = transactionService;

            Title = Resources.Languages.AppResource.TransactionsView_Title;

            FilterTransactionsVM = new FilterTransactionsContentViewModel();
            FilterTransactionsVM.FilterCommandRaised += FilterTransactionsVM_FilterCommandRaised;
        }

        private void FilterTransactionsVM_FilterCommandRaised(object? sender, (List<Transaction> filteredTeansactions, List<TransactionCategory> filteredCategories) e)
        {
            FilteredTransaction.Clear();
            foreach (var transaction in e.filteredTeansactions)
            {
                FilteredTransaction.Add(new TransactionListItem(transaction, e.filteredCategories.Find(c => c.Id.Equals(transaction.TransactionCategoryId))!));
            }
        }

        [RelayCommand]
        private async Task NavigatedTo()
        {
            _isNavigatedTo = true;
            FilterTransactionsVM.Hide();
            await Task.Delay(100);
        }

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

        [RelayCommand]
        private void GotoTransactionDetails(TransactionListItem item) => Shell.Current.GoToAsync($"{nameof(TransactionDetailView)}?id={item.Transaction.Id}").FireAndForgetSafeAsync(ModalDisplayHandler);

        [RelayCommand]
        private async Task DeleteTransaction(TransactionListItem item)
        {
            var confirmation = ConfirmationFactory.CreateDeleteConfirmation<Transaction>();
            await this.RunWithBusyFlagAndConfirmationAsync(async () =>
            {
                if (!await Remove(item))
                {
                    //TODO: Zasoby + poporawa obsługi błędów
                    await Shell.Current.DisplayAlert("Błąd", "Nie udało się usunąć transakcji.", "OK");
                }
            }, confirmation);
        }

        internal void ToggleSwipeView(SwipeView? swipeView)
        {
            if (CurrentOpenSwipeView != null && CurrentOpenSwipeView != swipeView)
            {
                CurrentOpenSwipeView.Close();
            }

            CurrentOpenSwipeView = swipeView;
        }

        private async Task<bool> Remove(TransactionListItem transaction)
        {
            bool result = await _transactionService.DeleteTransactionAsync(_budgetService.CurrentBudget, transaction.Transaction);
            if (result)
            {
                RemoveLocally(transaction);
            }
            return result;
        }

        private void RemoveLocally(TransactionListItem transaction)
        {
            FilteredTransaction.Remove(transaction);
        }

        private async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await FilterTransactionsVM.Refresh();
                //Initialize filtering transaction based on origin setup
                await FilterTransactionsVM.Filter();
            });
        }

        private async Task InitData()
        {
            await Task.CompletedTask;
        }
    }
}