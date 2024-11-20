
namespace BudgetHero.App.Utilities
{
    public class FadeManager
    {
        private const int ShowFormDuration = 250;
        private const int HideFormDuration = 250;

        private ContentView _contentView;
        private VisualElement _root;

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
                _root.FadeTo(1, ShowFormDuration, Easing.CubicOut)
            );
        }

        private async Task HideFormAsync()
        {
            await Task.WhenAll(
                _root.FadeTo(0, HideFormDuration, Easing.CubicIn)
            );

            _root.IsVisible = false;
        }

        internal void Initialize(bool isVisible)
        {
            _root.IsVisible = isVisible;
        }
    }
}
