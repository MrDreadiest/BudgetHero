using BudgetHero.App.Models;
using BudgetHero.App.Models.Configurations.Widgets;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Transactions;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details.Widgets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Content.Widgets
{
    public partial class LastTransactionsContentViewModel : WidgetContentViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        [ObservableProperty]
        public ObservableCollection<TransactionListItem> _filteredTransaction = new();

        private LastTransactionsConfiguration _configuration;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly IBudgetService _budgetService;
        private readonly IConfigurationService _configurationService;

        public LastTransactionsContentViewModel() : this(
            App.Services.GetService<IModalDisplayHandler>()!,
            App.Services.GetService<ITransactionService>()!,
            App.Services.GetService<ITransactionCategoryService>()!,
            App.Services.GetService<IBudgetService>()!,
            App.Services.GetService<IConfigurationService>()!)
        {
        }

        public LastTransactionsContentViewModel(IModalDisplayHandler displayHandler, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService, IBudgetService budgetService, IConfigurationService configurationService)
        {
            _displayHandler = displayHandler;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;
            _budgetService = budgetService;
            _configurationService = configurationService;

            _configuration = new LastTransactionsConfiguration();

            Title = AppResource.Widget_LastTransactionsContentView_Title;
            DetailViewPath = $"{nameof(LastTransactionsDetailView)}";
        }

        [RelayCommand]
        public override void NavigateToDetailView() => Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(ModalDisplayHandler);

        public override async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                LoadConfiguration();
                await Reload();
            });
        }

        private async Task Reload()
        {
            FilteredTransaction.Clear();

            List<TransactionCategory> categories = await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);
            List<Transaction> transactions = await _transactionService.GetLastTransactionsAsync(_budgetService.CurrentBudget.Id, _configuration.Count);

            foreach (Transaction transaction in transactions)
            {
                FilteredTransaction.Add(new TransactionListItem(transaction, categories.Find(c => c.Id.Equals(transaction.TransactionCategoryId))!));
            }
        }

        public override void LoadConfiguration()
        {
            var configuration = _configurationService.LoadConfiguration<LastTransactionsConfiguration>();

            if (configuration != null)
            {
                _configuration = configuration;
            }
        }
    }
}
