using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _title = string.Empty;
    }
}
