using BudgetHero.App.Models;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Widgets;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;



        public FastBalanceContentViewModel FastBalanceVM { get; }
        public FastReportContentViewModel FastReportVM { get; }
        public LastTransactionsContentViewModel LastTransactionsVM { get; }
        public ShortcutsContentViewModel ShortcutsVM { get; }

        [ObservableProperty]
        private Budget _budget;

        private bool _isNavigatedTo;
        private bool _dataLoaded;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IBudgetService _budgetService;

        private List<WidgetContentViewModelBase> _widgets;

        public DashboardViewModel(IModalDisplayHandler displayHandler, IBudgetService budgetService)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;

            Title = Resources.Languages.AppResource.DashboardView_Title;

            FastBalanceVM = new FastBalanceContentViewModel();
            FastReportVM = new FastReportContentViewModel();
            LastTransactionsVM = new LastTransactionsContentViewModel();
            ShortcutsVM = new ShortcutsContentViewModel();

            _widgets = new List<WidgetContentViewModelBase>()
            {
                FastBalanceVM,
                FastReportVM,
                LastTransactionsVM,
                ShortcutsVM
            };
        }

        [RelayCommand]
        private void NavigatedToBudgetDetails() => Shell.Current.GoToAsync($"{nameof(BudgetDetailView)}?id={Budget.Id}").FireAndForgetSafeAsync(ModalDisplayHandler);

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
            await this.RunWithBusyFlagAsync(async () =>
            {
                Budget = _budgetService.CurrentBudget;

                _widgets.ForEach(widget => { widget.Refresh().FireAndForgetSafeAsync(); });

                await Task.CompletedTask;
            });
        }

        private async Task InitData()
        {
            await Task.CompletedTask;
        }
    }
}
