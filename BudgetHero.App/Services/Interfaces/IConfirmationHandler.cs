using BudgetHero.App.Services.Common;

namespace BudgetHero.App.Services.Interfaces
{
    public interface IConfirmationHandler
    {
        Task<bool> HandleConfirmation(IConfirmation confirmation);
    }
}
