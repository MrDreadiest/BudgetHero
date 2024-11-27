using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Details.Widgets
{
    public abstract partial class WidgetDetailViewModelBase : ObservableObject, IConfigurable
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _title = string.Empty;

        public abstract Task Save();

        public abstract void LoadConfiguration();
        public abstract void SaveConfiguration();
    }
}
