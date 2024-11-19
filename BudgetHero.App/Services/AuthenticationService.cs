using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Account;

namespace BudgetHero.App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public event EventHandler? LoggedIn;
        public event EventHandler? LoggedOut;

        private readonly IApiClient _apiClient;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        public AuthenticationService(IApiClient apiClient, ITokenService tokenService, IUserService userService, IBudgetService budgetService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
        {
            _apiClient = apiClient;
            _tokenService = tokenService;
            _userService = userService;
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;
            _transactionService = transactionService;
        }

        public async Task<bool> LoginAsync(LoginRequestModel request)
        {
            var url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Login}";
            var response = await _apiClient.PostAsync<LoginRequestModel, LoginResponseModel>(url, request);

            if (response != null)
            {
                await _tokenService.SaveTokensAsync(response);
                await _userService.GetUserAsync();
                await _budgetService.TryGetLastBudgetAsCurrentAsync();
                if (!_budgetService.CurrentBudget.IsNullOrEmpty())
                {
                    var transactionTask = _transactionService.GetAllTransactionAsync(_budgetService.CurrentBudget);
                    var categoriesTask = _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);

                    await Task.WhenAll(transactionTask, categoriesTask);
                }
                LoggedIn?.Invoke(this, EventArgs.Empty);
            }
            return response is not null;
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                await _tokenService.RemoveTokensAsync();
                LoggedOut?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
