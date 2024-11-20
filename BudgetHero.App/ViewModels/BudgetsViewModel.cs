using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels
{
    public partial class BudgetsViewModel : ViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private bool _isNavigatedTo;
        private bool _dataLoaded;

        private readonly IModalDisplayHandler _displayHandler;

        public BudgetsViewModel(IModalDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;
            Title = Resources.Languages.AppResource.BudgetsView_Title;
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
            await Task.CompletedTask;
        }

        private async Task InitData()
        {
            await Task.CompletedTask;
        }
    }
}
