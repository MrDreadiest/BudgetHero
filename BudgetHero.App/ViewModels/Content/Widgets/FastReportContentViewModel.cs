using BudgetHero.App.Factories.Reports;
using BudgetHero.App.Models;
using BudgetHero.App.Models.Configurations.Widgets;
using BudgetHero.App.Models.Filter;
using BudgetHero.App.Models.Icon;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Reports;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details.Widgets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Widgets
{
    public partial class FastReportContentViewModel : WidgetContentViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;



        [ObservableProperty]
        private string _date;

        [ObservableProperty]
        private bool _isConfigurationValid;

        [ObservableProperty]
        private bool _isGraphEmpty;

        public ReportPieViewModel ReportPieViewModel { get; }
        public ReportTableViewModel ReportTableViewModel { get; }

        private FastReportConfiguration _configuration;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IConfigurationService _configurationService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        private readonly ReportViewModel _reportViewModel;


        public FastReportContentViewModel() : this(
            App.Services.GetService<IModalDisplayHandler>()!,
            App.Services.GetService<IConfigurationService>()!,
            App.Services.GetService<IBudgetService>()!,
            App.Services.GetService<ITransactionService>()!,
            App.Services.GetService<ITransactionCategoryService>()!)
        {
        }

        public FastReportContentViewModel(IModalDisplayHandler displayHandler, IConfigurationService configurationService, IBudgetService budgetService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService)
        {
            _displayHandler = displayHandler;
            _configurationService = configurationService;
            _budgetService = budgetService;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;

            _reportViewModel = new ReportViewModel();
            _configuration = new FastReportConfiguration();

            ReportPieViewModel = ReportFactoryProvider.GetFactory(Models.Report.ReportType.GraphPie).CreateReport(_reportViewModel) as ReportPieViewModel;
            ReportTableViewModel = ReportFactoryProvider.GetFactory(Models.Report.ReportType.Table).CreateReport(_reportViewModel) as ReportTableViewModel;

            Title = AppResource.Widget_FastReportContentView_Title;
            Date = DateTime.Now.ToString("MMMM yyyy");
            DetailViewPath = $"{nameof(FastReportDetailView)}";
            IsConfigurationValid = false;
            IsGraphEmpty = true;
        }

        [RelayCommand]
        public override void NavigateToDetailView() => Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(ModalDisplayHandler);

        public override async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                LoadConfiguration();

                if (IsConfigurationValid)
                {
                    var dateRange = DateFilterType.ThisMonth.GetDateRange();

                    List<TransactionCategory> allCategories = await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);

                    List<TransactionCategory> categories = new();
                    List<TransactionCategory> categoriesLeft = new();

                    List<Transaction> transactions = new();
                    List<Transaction> transactionsLeft = new();

                    Dictionary<string, Dictionary<string, decimal>> filteredDate = new();

                    Enum.TryParse(_configuration.SelectType, out TransactionCategoriesSelectType selectType);

                    switch (selectType)
                    {
                        case TransactionCategoriesSelectType.Own:
                            categories = allCategories
                                .Where(c => _configuration.SelectedCategoriesIds.Contains(c.Id))
                                .ToList();
                            break;
                        case TransactionCategoriesSelectType.TopCount:
                            categories = await _transactionCategoryService.GetTopCountTransactionCategoriesInDataRangeAsync(_budgetService.CurrentBudget.Id, _configuration.TopCounter, dateRange.DateFrom, dateRange.DateTo);
                            break;
                        case TransactionCategoriesSelectType.TopAmount:
                            categories = await _transactionCategoryService.GetTopAmountTransactionCategoriesInDataRangeAsync(_budgetService.CurrentBudget.Id, _configuration.TopCounter, dateRange.DateFrom, dateRange.DateTo);
                            break;
                        default:
                            break;
                    }

                    transactions = await _transactionService.GetTransactionInRangeByCategoriesAsync(
                        _budgetService.CurrentBudget.Id,
                        dateRange.DateFrom,
                        dateRange.DateTo,
                        categories.Select(c => c.Id).ToList());

                    IsGraphEmpty = transactions.Count == 0;

                    if (IsGraphEmpty)
                    {
                        IsConfigurationValid = false;
                        return;
                    }

                    if (_configuration.IsSumOtherAsLastSwitch)
                    {
                        categoriesLeft = allCategories
                            .Where(c => !categories.Any(cat => cat.Id == c.Id))
                            .ToList();

                        transactionsLeft = await _transactionService.GetTransactionInRangeByCategoriesAsync(
                            _budgetService.CurrentBudget.Id,
                            dateRange.DateFrom,
                            dateRange.DateTo,
                            categoriesLeft.Select(c => c.Id).ToList());

                        var localOtherTransactionCategory = new TransactionCategory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            BudgetId = string.Empty,
                            IconUnicode = Icons.Rest,
                            Name = IconHelper.GetName(Icons.Rest)
                        };

                        var localOtherTransaction = new Transaction()
                        {
                            TransactionCategoryId = localOtherTransactionCategory.Id,
                            BudgetId = string.Empty,
                            Id = string.Empty,
                            CreatorId = string.Empty,
                            TotalAmount = transactionsLeft.Sum(t => t.TotalAmount)
                        };

                        transactions.Add(localOtherTransaction);
                        categories.Add(localOtherTransactionCategory);
                    }

                    _reportViewModel.SetPercentageVisible(_configuration.IsShowPercentageSwitch);
                    _reportViewModel.UpdateData(transactions, categories);
                    _reportViewModel.GenerateCategoryColors();

                    ReportPieViewModel.DataPresentation().FireAndForgetSafeAsync();
                    ReportTableViewModel.DataPresentation().FireAndForgetSafeAsync();
                }
            });
        }

        public override void LoadConfiguration()
        {
            var configuration = _configurationService.LoadConfiguration<FastReportConfiguration>();
            IsConfigurationValid = false;
            IsGraphEmpty = true;

            if (configuration != null)
            {
                _configuration = configuration;

                if (_configuration.SelectedCategoriesIds.Count > 0 && _configuration.SelectType.Equals(TransactionCategoriesSelectType.Own.ToString()))
                    IsConfigurationValid = true;

                if (_configuration.TopCounter >= FastReportConfiguration.TopCounterMin &&
                    !_configuration.SelectType.Equals(TransactionCategoriesSelectType.Own.ToString()))
                    IsConfigurationValid = true;

                if (_configuration.IsSumOtherAsLastSwitch)
                    IsConfigurationValid = true;
            }
        }
    }
}
