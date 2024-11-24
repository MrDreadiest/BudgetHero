using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.Models.Filter
{
    public enum DateFilterType
    {
        ThisMonth,
        ThreeMonths,
        SixMonths,
        TwelveMonths,
        //ThisYear,
        Own,
    }

    public static class DateFilterTypeExtensions
    {
        public static string GetDescription(this DateFilterType filterType)
        {
            switch (filterType)
            {
                case DateFilterType.ThisMonth:
                    return AppResource.DateFilterType_ThisMonth;
                case DateFilterType.ThreeMonths:
                    return AppResource.DateFilterType_ThreeMonths;
                case DateFilterType.SixMonths:
                    return AppResource.DateFilterType_SixMonths;
                case DateFilterType.TwelveMonths:
                    return AppResource.DateFilterType_TwelveMonths;
                //case DateFilterType.ThisYear:
                //    return AppResource.DateFilterType_ThisYear;
                case DateFilterType.Own:
                    return AppResource.DateFilterType_Own;
                default:
                    return string.Empty;
            }
        }

        public static (DateTime DateFrom, DateTime DateTo) GetDateRange(this DateFilterType dateFilterType)
        {
            var today = DateTime.Today;

            var _dateFrom = DateTime.Now;
            var _dateTo = DateTime.Now;

            switch (dateFilterType)
            {
                case DateFilterType.ThisMonth:
                    _dateFrom = new DateTime(today.Year, today.Month, 1);
                    _dateTo = _dateFrom.AddMonths(1).AddDays(-1);
                    break;
                case DateFilterType.ThreeMonths:
                    _dateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-2);
                    _dateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;
                case DateFilterType.SixMonths:
                    _dateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-5);
                    _dateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;
                case DateFilterType.TwelveMonths:
                    _dateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-11);
                    _dateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;
                case DateFilterType.Own:
                    _dateFrom = today;
                    _dateTo = today;
                    break;
                default:
                    break;
            }
            return (_dateFrom, _dateTo);
        }
    }
}
