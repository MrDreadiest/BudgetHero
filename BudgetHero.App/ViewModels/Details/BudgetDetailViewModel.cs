using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Details
{
    public partial class BudgetDetailViewModel : ViewModelBase, IBusyHandler, IQueryAttributable
    {
        public IModalDisplayHandler? ModalDisplayHandler => throw new NotImplementedException();

        IBudgetService _budgetService;
        IUserService _userService;
        IModalDisplayHandler _modalDisplayHandler;

        public BudgetDetailViewModel(IBudgetService budgetService, IUserService userService, IModalDisplayHandler modalDisplayHandler)
        {
            _budgetService = budgetService;
            _userService = userService;
            _modalDisplayHandler = modalDisplayHandler;

            Title = AppResource.BudgetDetailView_Title;
        }

        [RelayCommand]
        private async Task EditBudget()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task DeleteBudget()
        {
            await Task.CompletedTask;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await LoadBudgetAsync(query);
            });
        }

        private async Task LoadBudgetAsync(IDictionary<string, object> query)
        {
            await Task.CompletedTask;
        }
    }
}
