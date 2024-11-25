using SkiaSharp;
using System.Globalization;

namespace BudgetHero.App.ViewModels.Converters
{
    public class SKColorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SKColor skColor)
            {

                return new Color(skColor.Red, skColor.Green, skColor.Blue, skColor.Alpha);
            }

            return new Color(1, 1, 1, 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
