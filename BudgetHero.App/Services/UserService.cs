using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.User;

namespace BudgetHero.App.Services
{
    public class UserService : IUserService
    {
        public event EventHandler? CurrentUserChanged;

        public User CurrentUser
        {
            get => _currentUser;
            private set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    CurrentUserChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private readonly IApiClient _apiClient;
        private User _currentUser;

        public UserService(IApiClient apiClient)
        {
            _apiClient = apiClient;
            _currentUser = new User() { Id = string.Empty };
        }

        public async Task<bool> GetUserAsync()
        {
            var url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Api}{ApiEndpoints.User}";
            var response = await _apiClient.GetAsync<UserGetResponseModel>(url);
            if (response != null)
            {
                CurrentUser = response.FromGetResponse();
                return true;
            }
            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var url = $"{ApiEndpoints.BaseAddress}{ApiEndpoints.Api}{ApiEndpoints.User}";
            var response = await _apiClient.PutAsync<UserUpdateRequestModel, UserUpdateResponseModel>(url, user.ToUpdateRequest());
            if (response != null)
            {
                _currentUser.FromUpdateResponse(response);
                CurrentUserChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public Task<bool> DeleteUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
