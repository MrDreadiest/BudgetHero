
namespace BudgetHero.App.Utilities
{
    public class FadeManager
    {
        private const int ShowFormDuration = 250;
        private const int HideFormDuration = 250;
        private const double ShowFormOpacity = 0;
        private const double HideFormOpacity = 1;


        private ContentView _contentView;
        private VisualElement _root;

        public uint ShowDuration = ShowFormDuration;
        public uint HideDuration = HideFormDuration;
        public double ShowOpacity = ShowFormOpacity;
        public double HideOpacity = HideFormOpacity;

        public FadeManager(ContentView contentView)
        {
            _contentView = contentView;
            _root = _contentView.FindByName<VisualElement>("Root");
            _root.IsVisible = false;
        }

        public async Task HandleCollapse(bool isVisible)
        {
            if (isVisible)
            {
                await ShowFormAsync();
            }
            else
            {
                await HideFormAsync();
            }
        }

        private async Task ShowFormAsync()
        {
            _root.IsVisible = true;
            _root.Opacity = 0;

            await Task.WhenAll(
                _root.FadeTo(HideOpacity, ShowDuration, Easing.CubicOut)
            );
        }

        private async Task HideFormAsync()
        {
            await Task.WhenAll(
                _root.FadeTo(ShowOpacity, HideDuration, Easing.CubicIn)
            );

            _root.IsVisible = false;
        }

        internal void Initialize(bool isVisible)
        {
            _root.IsVisible = isVisible;
        }
    }
}
