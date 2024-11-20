using Microsoft.Maui.Controls.Shapes;

namespace BudgetHero.App.Utilities
{
    public class CollapseManager
    {
        private const int ShowFormDuration = 250;
        private const int HideFormDuration = 250;

        public Border? Header { get; private set; }
        public Border? Body { get; private set; }

        public void Initialize(ContentView view, bool isCollapsed)
        {
            Header = view.FindByName<Border>("Header");
            Body = view.FindByName<Border>("Body");

            if (isCollapsed)
            {
                Body.IsVisible = false;
                Header.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) };
            }
            else
            {
                Body.IsVisible = true;
                Header.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10, 10, 0, 0) };
            }
        }

        public async Task HandleCollapse(bool isCollapsed)
        {
            if (!isCollapsed)
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
            if (Body != null)
            {
                Body.IsVisible = true;
                Body.Opacity = 0;

                await Task.WhenAll(
                    Body.FadeTo(1, ShowFormDuration, Easing.CubicOut),
                    AnimateCornerRadiusAsync(new CornerRadius(10), new CornerRadius(10, 10, 0, 0), ShowFormDuration)
                );
            }
        }

        private async Task HideFormAsync()
        {
            if (Body != null)
            {
                await Task.WhenAll(
                    Body.FadeTo(0, HideFormDuration, Easing.CubicIn),
                    AnimateCornerRadiusAsync(new CornerRadius(10, 10, 0, 0), new CornerRadius(10), HideFormDuration)
                );

                Body.IsVisible = false;
            }
        }

        private async Task AnimateCornerRadiusAsync(CornerRadius fromRadius, CornerRadius toRadius, uint duration = 300)
        {
            if (Header != null)
            {
                uint rate = 16;
                double step = rate / (double)duration;
                int delay = (int)(duration * step);
                double progress = 0;

                while (progress < 1)
                {
                    progress += step;

                    var currentRadius = new CornerRadius(
                        Lerp(fromRadius.TopLeft, toRadius.TopLeft, progress),
                        Lerp(fromRadius.TopRight, toRadius.TopRight, progress),
                        Lerp(fromRadius.BottomRight, toRadius.BottomRight, progress),
                        Lerp(fromRadius.BottomLeft, toRadius.BottomLeft, progress)
                    );

                    Header.StrokeShape = new RoundRectangle { CornerRadius = currentRadius };
                    await Task.Delay(delay);
                }

                Header.StrokeShape = new RoundRectangle { CornerRadius = toRadius };
            }
        }

        private double Lerp(double start, double end, double progress)
        {
            return start + (end - start) * progress;
        }
    }
}
