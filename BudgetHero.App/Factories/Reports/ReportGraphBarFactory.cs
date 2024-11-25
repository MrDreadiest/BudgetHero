using BudgetHero.App.ViewModels.Content.Reports;

namespace BudgetHero.App.Factories.Reports
{
    public class ReportGraphBarFactory : IReportFactory
    {
        public IReport CreateReport(ReportViewModel reportViewModel)
        {
            return new ReportBarViewModel(reportViewModel);
        }
    }
}
