using BudgetHero.App.ViewModels.Content.Reports;

namespace BudgetHero.App.Factories.Reports
{
    public interface IReportFactory
    {
        IReport CreateReport(ReportViewModel reportViewModel);
    }
}
