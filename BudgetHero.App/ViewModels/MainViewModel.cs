using BudgetHero.App.ViewModels.Content.Main;
using BudgetHero.App.ViewModels.Content.Universal;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Toolkit.SegmentedControl;

namespace BudgetHero.App.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private bool _isNavigatedTo;
        private bool _dataLoaded;

        public List<SfSegmentItem> SegmentItems { get; }
        public List<MainCarouselItemViewModelBase> MainCarouselVMs { get; }

        public SegmentedControlViewModel SegmentedControlVM { get; }

        [ObservableProperty]
        private int _carouselPosition;

        public MainViewModel()
        {
            MainCarouselVMs = new List<MainCarouselItemViewModelBase>()
            {
                new LoginContentViewModel(),
                new RegisterContentViewModel(),
            };

            SegmentedControlVM = new SegmentedControlViewModel(MainCarouselVMs.Select(item => new SegmentedControlItem(item.Title)).ToList());
            SegmentedControlVM.SelectionChanged += SegmentedControlVM_SelectionChanged;

            SegmentItems = MainCarouselVMs.Select(item => new SfSegmentItem() { Text = item.ItemViewType.GetDescription() }).ToList();
        }

        private void SegmentedControlVM_SelectionChanged(object? sender, (int oldIndex, int newIndex) e)
        {
            if (e.oldIndex != e.newIndex)
            {
                CarouselPosition = e.newIndex;
            }
        }

        [RelayCommand]
        private void NavigatedTo() => _isNavigatedTo = true;

        [RelayCommand]
        private void NavigatedFrom() => _isNavigatedTo = false;

        [RelayCommand]
        private async Task Appearing()
        {
            if (!_dataLoaded)
            {
                await InitData();
                _dataLoaded = true;
                await Refresh();
            }
            // This means we are being navigated to
            else if (!_isNavigatedTo)
            {
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            foreach (var item in MainCarouselVMs)
            {
                await item.ResetView();
            }
        }

        private async Task InitData()
        {
            await Task.CompletedTask;
        }
    }
}
