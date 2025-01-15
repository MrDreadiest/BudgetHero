using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Widgets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Shortcuts
{
    public partial class ShortcutContentViewModel : ObservableObject, IShortcut
    {
        [ObservableProperty]
        private string _detailViewPath = string.Empty;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _iconUnicode = string.Empty;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly ShortcutsContentViewModel _shortcutsVM;

        public ShortcutContentViewModel(string title, string iconUnicode, string detailViewPath, ShortcutsContentViewModel shortcutsVM) : this(App.Services.GetService<IModalDisplayHandler>()!, title, iconUnicode, detailViewPath, shortcutsVM)
        {
        }

        public ShortcutContentViewModel(IModalDisplayHandler displayHandler, string title, string iconUnicode, string detailViewPath, ShortcutsContentViewModel shortcutsVM)
        {
            _displayHandler = displayHandler;
            _shortcutsVM = shortcutsVM;

            Title = title;
            IconUnicode = iconUnicode;
            DetailViewPath = detailViewPath;
        }

        [RelayCommand]
        public async Task NavigateToDetailView()
        {
            await _shortcutsVM.Refresh();
            Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(_displayHandler);
        }
    }
}
