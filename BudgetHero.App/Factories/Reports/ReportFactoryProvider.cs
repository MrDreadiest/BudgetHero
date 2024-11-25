using BudgetHero.App.Models.Report;

namespace BudgetHero.App.Factories.Reports
{
    public static class ReportFactoryProvider
    {
        public static IReportFactory GetFactory(ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Table:
                    return new ReportTableFactory();
                case ReportType.GraphBar:
                    return new ReportGraphBarFactory();
                case ReportType.GraphPie:
                    return new ReportGraphPieFactory();
                default:
                    throw new ArgumentException("Unsupported report type");
            }
        }
    }
}
