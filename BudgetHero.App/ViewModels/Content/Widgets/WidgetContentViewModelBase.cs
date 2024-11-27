using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Content.Widgets
{
    public abstract partial class WidgetContentViewModelBase : ObservableObject, IConfigurable
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _detailViewPath = string.Empty;

        public abstract void NavigateToDetailView();

        public abstract void LoadConfiguration();
        public abstract void SaveConfiguration();

        public abstract Task Refresh();
    }
}
