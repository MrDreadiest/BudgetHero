namespace BudgetHero.App.Services.Common
{
    public interface IConfirmation
    {
        string Title { get; set; }
        string Message { get; set; }
        string Accept { get; set; }
        string Cancel { get; set; }
    }
}
