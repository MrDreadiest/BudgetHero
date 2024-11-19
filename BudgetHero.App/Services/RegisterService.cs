using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Account;

namespace BudgetHero.App.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IApiClient _apiClient;

        public RegisterService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task RegisterAsync(RegisterRequestModel requestModel)
        {
            var url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Register}";
            await _apiClient.PostAsync<RegisterRequestModel>(url, requestModel);
        }
    }
}
