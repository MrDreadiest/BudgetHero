using HomeBudget.Common.EntityDTOs.Account;

namespace BudgetHero.App.Services.Interfaces
{
    public interface IRegisterService
    {
        Task RegisterAsync(RegisterRequestModel requestModel);
    }
}
