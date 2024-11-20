using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Budget;

namespace BudgetHero.App.Services
{
    public class BudgetService : IBudgetService
    {
        public event EventHandler? CurrentBudgetChanged;
        public event EventHandler? BudgetsChanged;

        public Budget CurrentBudget
        {
            get => _currentBudget;
            set
            {
                if (_currentBudget != value)
                {
                    _currentBudget = value;
                    _appSettingsService.SetLastBudgetIdAsync(_currentBudget.Id);
                    CurrentBudgetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public IReadOnlyList<Budget> Budgets => _budgets.AsReadOnly();

        private readonly IApiClient _apiClient;
        private readonly IUserService _userService;
        private readonly IAppSettingsService _appSettingsService;

        private Budget _currentBudget;
        private List<Budget> _budgets;

        public BudgetService(IApiClient apiClient, IUserService userService, IAppSettingsService appSettingsService)
        {
            _apiClient = apiClient;
            _userService = userService;
            _appSettingsService = appSettingsService;

            _currentBudget = new Budget() { Id = string.Empty, OwnerId = string.Empty };
            _budgets = new List<Budget>();
        }

        public async Task TryGetLastBudgetAsCurrentAsync()
        {
            await GetAllBudgetsAsync();
            string lastBudgetId = await _appSettingsService.GetLastBudgetIdAsync();
            SelectBudgetAsCurrent(lastBudgetId);
        }

        public bool SelectBudgetAsCurrent(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var budget = Budgets.FirstOrDefault(b => b.Id == id);

                if (budget != null)
                {
                    if (budget.Id.Equals(id))
                    {
                        CurrentBudget = budget;
                        return true;
                    }
                }
            }
            CurrentBudget = new Budget() { Id = string.Empty, OwnerId = string.Empty };
            return false;
        }

        public async Task<bool> GetAllBudgetsAsync()
        {
            string url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Api}{ApiEndpoints.User}{ApiEndpoints.Budget}";
            var budgets = await _apiClient.GetAsync<IEnumerable<BudgetGetResponseModel>>(url);

            if (budgets != null)
            {
                _budgets = budgets.Select(b => b.FromGetResponse()).ToList();
                BudgetsChanged?.Invoke(this, EventArgs.Empty);
            }

            return budgets != null;
        }

        public async Task<Budget?> GetBudgetByIdAsync(string budgetId)
        {
            string url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Api}{ApiEndpoints.User}{ApiEndpoints.Budget}/{budgetId}";
            var budgetResponse = await _apiClient.GetAsync<BudgetGetResponseModel>(url);

            return budgetResponse == null ? null : budgetResponse.FromGetResponse();
        }

        public async Task<bool> CreateBudgetAsync(Budget budget)
        {
            string url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Api}{ApiEndpoints.User}{ApiEndpoints.Budget}";
            var response = await _apiClient.PostAsync<BudgetCreateRequestModel, BudgetCreateResponseModel>(url, budget.ToCreateRequest());
            if (response != null)
            {
                Budget newBudget = response.FromCreateResponse();
                newBudget.Users.Add(_userService.CurrentUser);
                _budgets.Add(newBudget);
                BudgetsChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public Task<bool> DeleteBudgetAsync(string budgetId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBudgetAsync(Budget budget)
        {
            throw new NotImplementedException();
        }


    }
}
