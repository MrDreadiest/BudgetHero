namespace BudgetHero.App.ViewModels.Content.Shortcuts
{
    public interface IShortcut
    {
        string DetailViewPath { get; set; }
        string Title { get; set; }
        string IconUnicode { get; set; }
        Task NavigateToDetailView();
    }
}
