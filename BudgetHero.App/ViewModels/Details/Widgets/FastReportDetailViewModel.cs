using BudgetHero.App.Factories.Confirmations;
using BudgetHero.App.Models;
using BudgetHero.App.Models.Configurations.Widgets;
using BudgetHero.App.Models.Filter;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Details.Widgets
{
    public partial class FastReportDetailViewModel : WidgetDetailViewModelBase, IBusyHandler
    {
        [ObservableProperty]
        private int _topCounter;

        [ObservableProperty]
        private bool _isShowPercentageSwitch;

        [ObservableProperty]
        private bool _isSumOtherAsLastSwitch;

        [ObservableProperty]
        private bool _isTopCounterAvailable;

        public ObservableCollection<TransactionCategory> SelectedCategories { get; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;
        public SegmentedControlViewModel SegmentedControlVM { get; }
        public DropdownTransactionCategoryContentViewModel DropdownTransactionCategoryVM { get; }

        private TransactionCategoriesSelectType _transactionCategoriesSelectType;
        private FastReportConfiguration _configuration;

        private bool _isNavigatedTo;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IConfigurationService _configurationService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public FastReportDetailViewModel(IModalDisplayHandler displayHandler, IConfigurationService configurationService, IBudgetService budgetService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;
            _configurationService = configurationService;

            _configuration = new FastReportConfiguration();

            Title = AppResource.Widget_FastReportDetailView_Title;

            SegmentedControlVM = new SegmentedControlViewModel(Enum.GetValues(typeof(TransactionCategoriesSelectType)).Cast<TransactionCategoriesSelectType>().Select(f => new SegmentedControlItem(f.GetDescription())).ToList());
            SegmentedControlVM.SelectionChanged += SegmentedControlVM_SelectionChanged;

            DropdownTransactionCategoryVM = new DropdownTransactionCategoryContentViewModel(true, false);
            DropdownTransactionCategoryVM.SelectedTransactionCategoryChanged += DropdownTransactionCategoryVM_SelectedTransactionCategoryChanged;

            SelectedCategories = new();
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
        public override async Task Save()
        {
            var confirmation = ConfirmationFactory.CreateUpdateConfirmation<FastReportConfiguration>();
            await this.RunWithBusyFlagAndConfirmationAsync(async () =>
            {
                SaveConfiguration();
                await Task.CompletedTask;
            }, confirmation);
        }

        public override void LoadConfiguration()
        {
            var configuration = _configurationService.LoadConfiguration<FastReportConfiguration>();

            if (configuration != null)
            {
                _configuration = configuration;
            }

            Enum.TryParse(_configuration.SelectType, out TransactionCategoriesSelectType selectType);
            SegmentedControlVM.Select((int)selectType);

            TopCounter = _configuration.TopCounter;
            IsShowPercentageSwitch = _configuration.IsShowPercentageSwitch;
            IsSumOtherAsLastSwitch = _configuration.IsSumOtherAsLastSwitch;

            SegmentedControlVM.Select(SegmentedControlVM.Items.ToList().Find(o => o.Text == selectType.GetDescription())!);
            DropdownTransactionCategoryVM.SelectCategories(_configuration.SelectedCategoriesIds);
        }

        public override void SaveConfiguration()
        {
            _configuration.TopCounter = TopCounter;
            _configuration.IsShowPercentageSwitch = IsShowPercentageSwitch;
            _configuration.IsSumOtherAsLastSwitch = IsSumOtherAsLastSwitch;
            _configuration.SelectType = _transactionCategoriesSelectType.ToString();
            _configuration.SelectedCategoriesIds = SelectedCategories.Select(c => c.Id).ToList();

            _configurationService.SaveConfiguration(_configuration);
        }

        private async Task Refresh()
        {
            LoadConfiguration();
            await DropdownTransactionCategoryVM.Refresh();
        }

        private void SegmentedControlVM_SelectionChanged(object? sender, (int oldIndex, int newIndex) e)
        {
            if (e.oldIndex != e.newIndex)
            {
                _transactionCategoriesSelectType = (TransactionCategoriesSelectType)e.newIndex;
            }

            IsTopCounterAvailable = _transactionCategoriesSelectType != TransactionCategoriesSelectType.Own;

            if (IsTopCounterAvailable)
            {
                TopCounter = FastReportConfiguration.DefaultTopTransactionCount;
                DropdownTransactionCategoryVM.Reset().FireAndForgetSafeAsync();
            }
            else
            {
                TopCounter = 0;
            }

            DropdownTransactionCategoryVM.IsEnable = !IsTopCounterAvailable;
        }

        private void DropdownTransactionCategoryVM_SelectedTransactionCategoryChanged(object? sender, List<Models.TransactionCategory> e)
        {
            SelectedCategories.Clear();
            e.ForEach(x => SelectedCategories.Add(x));
        }
    }
}
