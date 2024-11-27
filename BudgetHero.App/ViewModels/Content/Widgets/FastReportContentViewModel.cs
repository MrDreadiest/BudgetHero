using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details.Widgets;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Widgets
{
    public partial class FastReportContentViewModel : WidgetContentViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IModalDisplayHandler _displayHandler;

        public FastReportContentViewModel() : this(App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public FastReportContentViewModel(IModalDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;

            Title = AppResource.Widget_FastReportContentView_Title;
            DetailViewPath = $"{nameof(FastReportDetailView)}";
        }

        [RelayCommand]
        public override void NavigateToDetailView() => Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(ModalDisplayHandler);

        public override async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await Task.Delay(1500);
            });
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
