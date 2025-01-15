using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Details.Shortcuts
{
    public partial class ManageCategoriesDetailViewModel : ViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _modalDisplayHandler;

        private bool _isNavigatedTo;

        private readonly IModalDisplayHandler _modalDisplayHandler;

        public ManageCategoriesDetailViewModel(IModalDisplayHandler modalDisplayHandler)
        {
            _modalDisplayHandler = modalDisplayHandler;

            //TODO: Zasoby
            Title = "Manage Categories";
        }

        [RelayCommand]
        private void NavigatedTo() => _isNavigatedTo = true;

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

        private async Task Refresh()
        {
            await Task.CompletedTask;
        }

    }
}
