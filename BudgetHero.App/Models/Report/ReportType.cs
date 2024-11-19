using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.Models.Report
{
    public enum ReportType
    {
        Table,
        GraphBar,
        GraphPie
    }

    public static class ReportTypeExtensions
    {
        public static string GetDescription(this ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Table:
                    return AppResource.ReportType_Table;
                case ReportType.GraphBar:
                    return AppResource.ReportType_GraphBar;
                case ReportType.GraphPie:
                    return AppResource.ReportType_GraphPie;
                default:
                    return string.Empty;
            }
        }
    }
}
