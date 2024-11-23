using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Details
{
    public partial class TransactionDetailViewModel : ViewModelBase, IBusyHandler, IQueryAttributable
    {
        public IModalDisplayHandler? ModalDisplayHandler => _modalDisplayHandler;

        IBudgetService _budgetService;
        ITransactionService _transactionService;
        ITransactionCategoryService _transactionCategoryService;
        IModalDisplayHandler _modalDisplayHandler;

        public TransactionDetailViewModel(IBudgetService budgetService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService, IModalDisplayHandler modalDisplayHandler)
        {
            _budgetService = budgetService;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;
            _modalDisplayHandler = modalDisplayHandler;

            Title = AppResource.TransactionDetailView_Title;
        }

        [RelayCommand]
        private async Task EditTransaction()
        {

        }

        [RelayCommand]
        private async Task DeleteTransaction()
        {

        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            LoadTaskAsync(query).FireAndForgetSafeAsync();
        }

        private async Task LoadTaskAsync(IDictionary<string, object> query)
        {
            await Task.CompletedTask;
        }
    }
}
