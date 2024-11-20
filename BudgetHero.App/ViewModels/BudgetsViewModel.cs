using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels
{
    public partial class BudgetsViewModel : ViewModelBase, IBusyHandler
    {
        public AddBudgetContentViewModel AddBudgetVM { get; private set; }
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private bool _isNavigatedTo;
        private bool _dataLoaded;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IBudgetService _budgetService;

        public BudgetsViewModel(IModalDisplayHandler displayHandler, IBudgetService budgetService)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;

            _budgetService.BudgetsChanged += BudgetService_BudgetsChanged;

            Title = Resources.Languages.AppResource.BudgetsView_Title;

            AddBudgetVM = new AddBudgetContentViewModel();
        }

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
            AddBudgetVM.Refresh();
            await Task.CompletedTask;
        }

        private async Task InitData()
        {
            await Task.CompletedTask;
        }

        private void BudgetService_BudgetsChanged(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
