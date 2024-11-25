using BudgetHero.App.ViewModels.Content.Reports;

namespace BudgetHero.App.Factories.Reports
{
    public class ReportGraphPieFactory : IReportFactory
    {
        public IReport CreateReport(ReportViewModel reportViewModel)
        {
            return new ReportPieViewModel(reportViewModel);
        }
    }
}
