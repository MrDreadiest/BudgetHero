using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Content.FirstSetup
{
    public abstract partial class FirstSetupCarouselItemViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _title = string.Empty;

        public FirstSetupCarouselItemViewsType ItemViewType { get; }

        public abstract Task OnAppearingAsync();
        public abstract Task OnDisappearingAsync();
        public abstract Task ResetViewAsync();

        public abstract bool CanGoNext();

        protected FirstSetupCarouselItemViewModelBase(FirstSetupCarouselItemViewsType itemViewType)
        {
            ItemViewType = itemViewType;
            Title = itemViewType.GetDescriptions();
        }
    }
}
