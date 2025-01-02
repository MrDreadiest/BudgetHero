
using BudgetHero.App.Factories.Shortcuts;
using BudgetHero.App.Models.Shortcuts;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Shortcuts;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details.Widgets;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Content.Widgets
{
    public partial class ShortcutsContentViewModel : WidgetContentViewModelBase, IBusyHandler
    {

        public ObservableCollection<ShortcutContentViewModel> Shortcuts { get; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IModalDisplayHandler _displayHandler;

        public ShortcutsContentViewModel() : this(App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public ShortcutsContentViewModel(IModalDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;

            Shortcuts = new ObservableCollection<ShortcutContentViewModel>()
            {
                (ShortcutContentViewModel)ShortcutFactory.CreateShortcut(ShortcutType.AddTransaction),
                (ShortcutContentViewModel)ShortcutFactory.CreateShortcut(ShortcutType.SplitTransaction),
                (ShortcutContentViewModel)ShortcutFactory.CreateShortcut(ShortcutType.ManageCategories),
            };

            Title = AppResource.Widget_ShortcutsContentView_Title;
            DetailViewPath = $"{nameof(ShortcutsDetailView)}";
        }

        [RelayCommand]
        public override void NavigateToDetailView() => Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(ModalDisplayHandler);

        public override async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await Task.CompletedTask;
            });
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
