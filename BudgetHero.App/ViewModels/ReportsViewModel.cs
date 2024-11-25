using BudgetHero.App.Factories.Reports;
using BudgetHero.App.Models;
using BudgetHero.App.Models.Icon;
using BudgetHero.App.Models.Report;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Reports;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels
{
    public partial class ReportsViewModel : ViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        public ObservableCollection<ReportCarouselItemViewModelBase> ReportsCarouselVMs { get; }

        public FilterTransactionsContentViewModel FilterTransactionsVM { get; }

        public SegmentedControlViewModel SegmentedControlVM { get; }

        [ObservableProperty]
        private int _carouselPosition;

        [ObservableProperty]
        private decimal _allTransactionsAmount;

        [ObservableProperty]
        private decimal _avgTransactionsAmount;

        [ObservableProperty]
        private bool _isPercentageVisible;

        [ObservableProperty]
        private bool _isSumOtherAsLastSwitch;

        [ObservableProperty]
        private bool _isCategoryNameVisible;

        private bool _isNavigatedTo;
        private bool _dataLoaded;

        private readonly ReportViewModel _reportViewModel;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        private readonly IModalDisplayHandler _displayHandler;

        public ReportsViewModel(IBudgetService budgetService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService, IModalDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;

            FilterTransactionsVM = new FilterTransactionsContentViewModel();
            FilterTransactionsVM.FilterCommandRaised += FilterTransactionsVM_FilterCommandRaised;

            _reportViewModel = new ReportViewModel();

            ReportsCarouselVMs = new ObservableCollection<ReportCarouselItemViewModelBase>()
            {
                (ReportCarouselItemViewModelBase)ReportFactoryProvider.GetFactory(ReportType.GraphBar).CreateReport(_reportViewModel),
                (ReportCarouselItemViewModelBase)ReportFactoryProvider.GetFactory(ReportType.Table).CreateReport(_reportViewModel)
            };

            SegmentedControlVM = new SegmentedControlViewModel(
                ReportsCarouselVMs.Select(report => new SegmentedControlItem(report.Title)).ToList());

            SegmentedControlVM.SelectionChanged += SegmentedControlVM_SelectionChanged;

            Title = Resources.Languages.AppResource.ReportsView_Title;
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

        private async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await FilterTransactionsVM.Refresh();
                //TODO: Odczytanie ostatnich ustawień filtracji
                //await FilterTransactionsVM.Filter();

                IsPercentageVisible = _reportViewModel.IsPercentageVisible;
                IsCategoryNameVisible = _reportViewModel.IsCategoryNameVisible;
            });
        }

        private async Task InitData()
        {
            await Task.CompletedTask;
        }

        partial void OnIsPercentageVisibleChanged(bool oldValue, bool newValue)
        {
            _reportViewModel.SetPercentageVisible(newValue);
        }

        partial void OnIsCategoryNameVisibleChanged(bool oldValue, bool newValue)
        {
            _reportViewModel?.SetCategoryNameVisible(newValue);
        }

        private void SegmentedControlVM_SelectionChanged(object? sender, (int oldIndex, int newIndex) e)
        {
            if (e.oldIndex != e.newIndex)
            {
                CarouselPosition = e.newIndex;
            }
        }

        private async void FilterTransactionsVM_FilterCommandRaised(object? sender, (List<Models.Transaction> filteredTeansactions, List<Models.TransactionCategory> filteredCategories) e)
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                if (IsSumOtherAsLastSwitch)
                {
                    List<Transaction> transactionsLeft = new();
                    List<TransactionCategory> categoriesLeft = new();

                    categoriesLeft = _budgetService.CurrentBudget.TransactionCategories
                        .Where(c => !e.filteredCategories.Contains(c))
                        .ToList();

                    transactionsLeft = await _transactionService.GetTransactionInRangeByCategoriesAsync(
                        _budgetService.CurrentBudget.Id,
                        FilterTransactionsVM.DateFrom,
                        FilterTransactionsVM.DateTo,
                        categoriesLeft.Select(c => c.Id).ToList());

                    var localOtherTransactionCategory = new TransactionCategory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        BudgetId = string.Empty,
                        IconUnicode = Icons.Rest,
                        Name = IconHelper.GetName(Icons.Rest)
                    };

                    var localOtherTransactions = new List<Transaction>();

                    transactionsLeft.ForEach(transaction =>
                    {
                        transaction.TransactionCategoryId = localOtherTransactionCategory.Id;
                        e.filteredTeansactions.Add(transaction);
                    });

                    e.filteredCategories.Add(localOtherTransactionCategory);
                }

                _reportViewModel.UpdateData(e.filteredTeansactions, e.filteredCategories);
                _reportViewModel.GenerateCategoryColors();

                AllTransactionsAmount = _reportViewModel.AllTransactionsAmount;
                AvgTransactionsAmount = _reportViewModel.AvgTransactionsAmount;
            });

            foreach (var item in ReportsCarouselVMs)
            {
                if (item is IReport report)
                {
                    report.DataPresentation().FireAndForgetSafeAsync(ModalDisplayHandler);
                }
            }

            //TODO: Zapisanie ustaweiń filtracji
        }
    }
}
