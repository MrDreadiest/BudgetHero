namespace BudgetHero.App.ViewModels.Content.Reports
{
    public class ReportCarouselItemTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate ReportTableDT { get; set; }
        public required DataTemplate ReportGraphPieDT { get; set; }
        public required DataTemplate ReportGraphBarDT { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ReportTableViewModel)
                return ReportTableDT;
            else if (item is ReportBarViewModel)
                return ReportGraphBarDT;
            else if (item is ReportPieViewModel)
                return ReportGraphPieDT;
            return null;
        }
    }
}
