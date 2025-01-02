using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
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

        public ShortcutContentViewModel(string title, string iconUnicode, string detailViewPath) : this(App.Services.GetService<IModalDisplayHandler>()!, title, iconUnicode, detailViewPath)
        {
        }

        public ShortcutContentViewModel(IModalDisplayHandler displayHandler, string title, string iconUnicode, string detailViewPath)
        {
            _displayHandler = displayHandler;

            Title = title;
            IconUnicode = iconUnicode;
            DetailViewPath = detailViewPath;
        }

        [RelayCommand]
        public void NavigateToDetailView() => Shell.Current.GoToAsync($"{DetailViewPath}").FireAndForgetSafeAsync(_displayHandler);
    }
}
