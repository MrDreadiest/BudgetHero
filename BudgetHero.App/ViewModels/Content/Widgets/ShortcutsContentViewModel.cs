
using BudgetHero.App.Factories.Shortcuts;
using BudgetHero.App.Models.Shortcuts;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Shortcuts;
using BudgetHero.App.ViewModels.Interfaces;
using BudgetHero.App.Views.Details.Widgets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Content.Widgets
{
    public partial class ShortcutsContentViewModel : WidgetContentViewModelBase, IBusyHandler
    {
        private const int AnimationSpeed = 250;

        public ObservableCollection<ShortcutContentViewModel> Shortcuts { get; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IModalDisplayHandler _displayHandler;

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private uint _animationDuration;

        public ShortcutsContentViewModel() : this(App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public ShortcutsContentViewModel(IModalDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;

            Shortcuts = new ObservableCollection<ShortcutContentViewModel>()
            {
                (ShortcutContentViewModel)ShortcutFactory.CreateShortcut(ShortcutType.AddTransaction, this),
                (ShortcutContentViewModel)ShortcutFactory.CreateShortcut(ShortcutType.SplitTransaction, this),
                (ShortcutContentViewModel)ShortcutFactory.CreateShortcut(ShortcutType.ManageCategories, this),
            };

            Title = AppResource.Widget_ShortcutsContentView_Title;
            DetailViewPath = $"{nameof(ShortcutsDetailView)}";

            AnimationDuration = AnimationSpeed;
        }

        [RelayCommand]
        public override void NavigateToDetailView() => Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(ModalDisplayHandler);

        public override async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                IsOpen = false;
                await Task.Delay(AnimationSpeed * 2);
            });
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }

    }
}
