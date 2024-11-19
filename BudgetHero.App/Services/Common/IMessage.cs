namespace BudgetHero.App.Services.Common
{
    public interface IMessage
    {
        string Title { get; set; }
        string Body { get; set; }
        string Accept { get; }
    }
}
