using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Content.Reports
{
    public abstract partial class ReportCarouselItemViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isCollapsed;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private string _route = string.Empty;

        [ObservableProperty]
        private string _title = string.Empty;

        internal bool _isInitialized;

        public ReportCarouselItemViewModelBase()
        {
        }
    }
}
