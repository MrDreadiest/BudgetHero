using BudgetHero.App.Models;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Budget;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels
{
    public partial class BudgetsViewModel : ViewModelBase, IBusyHandler
    {

        public SegmentedControlViewModel SegmentedControlVM { get; }

        public List<BudgetListContentViewModel> BudgetsListVMs { get; }

        public AddBudgetContentViewModel AddBudgetVM { get; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        [ObservableProperty]
        private int _carouselPosition;

        private bool _isNavigatedTo;
        private bool _dataLoaded;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IBudgetService _budgetService;

        public BudgetsViewModel(IModalDisplayHandler displayHandler, IBudgetService budgetService)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;

            _budgetService.BudgetCreated += BudgetService_BudgetCreated;

            Title = Resources.Languages.AppResource.BudgetsView_Title;

            BudgetsListVMs = new List<BudgetListContentViewModel>()
            {
                new BudgetListContentViewModel(BudgetListType.Own),
                new BudgetListContentViewModel(BudgetListType.Shared)
            };

            SegmentedControlVM = new SegmentedControlViewModel(BudgetsListVMs.Select(item => new SegmentedControlItem(item.BudgetListType.GetDescription())).ToList());
            SegmentedControlVM.SelectionChanged += SegmentedControlVM_SelectionChanged;
            AddBudgetVM = new AddBudgetContentViewModel();
        }

        private void SegmentedControlVM_SelectionChanged(object? sender, (int oldIndex, int newIndex) e)
        {
            if (e.oldIndex != e.newIndex)
            {
                CarouselPosition = e.newIndex;
            }
        }

        [RelayCommand]
        private void NavigatedTo() => _isNavigatedTo = true;

        [RelayCommand]
        private void NavigatedFrom() => _isNavigatedTo = false;

        [RelayCommand]
        private async Task Appearing()
        {
            await this.RunWithBusyFlagAsync(async () =>
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
            });
        }

        private async Task Refresh()
        {
            AddBudgetVM.Refresh();
            BudgetsListVMs.ForEach(list => list.Clear());

            var ownList = BudgetsListVMs.Find(i => i.BudgetListType == BudgetListType.Own);
            foreach (var budget in _budgetService.OwnBudgets)
            {
                ownList!.Add(budget);
            }
            await Task.CompletedTask;
        }

        private async Task InitData()
        {
            await _budgetService.GetAllBudgetsAsync();
        }

        private void BudgetService_BudgetCreated(object? sender, Budget e)
        {
            BudgetsListVMs.Find(i => i.BudgetListType == BudgetListType.Own)!.Add(e);
        }
    }
}
