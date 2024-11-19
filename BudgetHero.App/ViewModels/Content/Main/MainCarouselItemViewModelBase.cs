using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Content.Main
{
    public abstract partial class MainCarouselItemViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _title = string.Empty;

        public MainCarouselItemViewType ItemViewType { get; }

        public abstract Task ResetView();

        protected MainCarouselItemViewModelBase(MainCarouselItemViewType itemViewType)
        {
            ItemViewType = itemViewType;
            Title = itemViewType.GetDescription();
        }
    }
}
