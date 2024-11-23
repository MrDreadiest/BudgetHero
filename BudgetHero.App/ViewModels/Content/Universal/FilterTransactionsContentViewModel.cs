using BudgetHero.App.Models;
using BudgetHero.App.Models.Filter;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Universal
{
    public partial class FilterTransactionsContentViewModel : ObservableObject, IBusyHandler
    {
        public event EventHandler<(List<Transaction> filteredTeansactions, List<TransactionCategory> filteredCategories)>? FilterCommandRaised;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        public SegmentedControlViewModel SegmentedControlVM { get; }

        public DropdownTransactionCategoryContentViewModel DropdownTransactionCategoryVM { get; }

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private decimal _priceFrom;

        [ObservableProperty]
        private decimal _priceTo;

        [ObservableProperty]
        private DateTime _dateFrom;

        [ObservableProperty]
        private DateTime _dateTo;

        [ObservableProperty]
        private bool _isDateEntryEnable;

        private List<TransactionCategory> _filteredCategories = new();

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        public FilterTransactionsContentViewModel() : this(
            App.Services.GetService<IModalDisplayHandler>()!,
            App.Services.GetService<IBudgetService>()!,
            App.Services.GetService<ITransactionCategoryService>()!,
            App.Services.GetService<ITransactionService>()!
            )
        {
        }

        public FilterTransactionsContentViewModel(IModalDisplayHandler displayHandler, IBudgetService budgetService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;
            _transactionService = transactionService;

            Title = AppResource.FilterTransactionsContentView_Title;

            SegmentedControlVM = new SegmentedControlViewModel(Enum.GetValues(typeof(DateFilterType)).Cast<DateFilterType>().Select(f => new SegmentedControlItem(f.GetDescription())).ToList());
            SegmentedControlVM.SelectionChanged += SegmentedControlVM_SelectionChanged;

            DropdownTransactionCategoryVM = new DropdownTransactionCategoryContentViewModel(true, false);
            DropdownTransactionCategoryVM.SelectedTransactionCategoryChanged += DropdownTransactionCategoryVM_SelectedTransactionCategoryChanged;

            var dateRange = ((DateFilterType)SegmentedControlVM.Items.IndexOf(SegmentedControlVM.SelectedItem)).GetDateRange();
            DateFrom = dateRange.DateFrom;
            DateTo = dateRange.DateTo;
        }

        private void DropdownTransactionCategoryVM_SelectedTransactionCategoryChanged(object? sender, List<Models.TransactionCategory> e)
        {
            _filteredCategories = e;
        }

        public async Task Refresh()
        {
            await DropdownTransactionCategoryVM.Refresh();
        }

        public async Task InitData()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task Reset()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await DropdownTransactionCategoryVM.Reset();
            });
        }

        [RelayCommand]
        public async Task Filter()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                var dateFilterType = (DateFilterType)SegmentedControlVM.Items.IndexOf(SegmentedControlVM.SelectedItem);

                List<Transaction> filteredTransaction = new();
                List<TransactionCategory> filteredCategories = new(_filteredCategories);

                if (filteredCategories.Count == 0)
                {
                    filteredTransaction = await _transactionService.GetTransactionInRangeAsync(_budgetService.CurrentBudget.Id, DateFrom, DateTo);
                    filteredCategories = DropdownTransactionCategoryVM.AllTransactionCategories;
                }
                else
                {
                    List<string> ids = filteredCategories.Select(category => category.Id).ToList();

                    filteredTransaction = await _transactionService.GetTransactionInRangeByCategoriesAsync(_budgetService.CurrentBudget.Id, DateFrom, DateTo, ids);
                }

                if (PriceFrom != PriceTo)
                {
                    if (PriceFrom > 0 && PriceTo == 0)
                    {
                        filteredTransaction = filteredTransaction.Where(transaction =>
                        transaction.TotalAmount >= PriceFrom &&
                        transaction.TotalAmount <= decimal.MaxValue
                        ).ToList();
                    }
                    else
                    {
                        filteredTransaction = filteredTransaction.Where(transaction =>
                        transaction.TotalAmount >= PriceFrom &&
                        transaction.TotalAmount <= PriceTo
                        ).ToList();
                    }
                }

                IsOpen = false;
                FilterCommandRaised?.Invoke(this, (filteredTransaction, filteredCategories));
            });
        }

        private void SegmentedControlVM_SelectionChanged(object? sender, (int oldIndex, int newIndex) e)
        {
            if (e.oldIndex != e.newIndex)
            {
                var dateFilterType = (DateFilterType)e.newIndex;
                var result = dateFilterType.GetDateRange();

                DateFrom = result.DateFrom;
                DateTo = result.DateTo;

                IsDateEntryEnable = dateFilterType == DateFilterType.Own;
            }
        }
    }
}
