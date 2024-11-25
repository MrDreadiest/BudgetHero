
using BudgetHero.App.ViewModels.Content.Reports;

namespace BudgetHero.App.Factories.Reports
{
    public class ReportTableFactory : IReportFactory
    {
        public IReport CreateReport(ReportViewModel reportViewModel)
        {
            return new ReportTableViewModel(reportViewModel);
        }
    }
}
