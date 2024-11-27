using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details.Widgets;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Widgets
{
    public partial class FastBalanceContentViewModel : WidgetContentViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IModalDisplayHandler _displayHandler;

        public FastBalanceContentViewModel() : this(App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public FastBalanceContentViewModel(IModalDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;

            Title = AppResource.Widget_FastBalanceContentView_Title;
            DetailViewPath = $"{nameof(FastBalanceDetailView)}";
        }

        [RelayCommand]
        public override void NavigateToDetailView() => Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(ModalDisplayHandler);

        public override async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await Task.Delay(1000);
            });
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
