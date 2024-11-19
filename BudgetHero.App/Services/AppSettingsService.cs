using BudgetHero.App.Services.Interfaces;

namespace BudgetHero.App.Services
{
    class AppSettingsService : IAppSettingsService
    {
        private const string RememberedUserEmail = "RememberedUserEmail";
        private const string RememberedUserPassword = "RememberedUserPassword ";
        private const string LastBudgetId = "LastBudgetId";
        private const string RememberedSwitchOn = "RememberedSwitchOn";

        private readonly ISecureStorage _secureStorage;

        public AppSettingsService()
        {
            _secureStorage = SecureStorage.Default;
        }

        public async Task DeleteIsRememberedSwitchOnAsync()
        {
            await Task.Run(() =>
            {
                _secureStorage.Remove(RememberedSwitchOn);
            });
        }

        public async Task DeleteLastBudgetIdAsync()
        {
            await Task.Run(() =>
            {
                _secureStorage.Remove(LastBudgetId);
            });
        }

        public async Task DeleteRememberedUserEmailAsync()
        {
            await Task.Run(() =>
            {
                _secureStorage.Remove(RememberedUserEmail);
            });
        }

        public async Task DeleteRememberedUserPasswordAsync()
        {
            await Task.Run(() =>
            {
                _secureStorage.Remove(RememberedUserPassword);
            });
        }

        public async Task<bool> GetIsRememberedSwitchOnAsync()
        {
            var stringState = await _secureStorage.GetAsync(RememberedSwitchOn);
            return string.IsNullOrEmpty(stringState) ? false : bool.Parse(stringState);
        }

        public async Task<string> GetLastBudgetIdAsync()
        {
            var budgetId = await _secureStorage.GetAsync(LastBudgetId);
            return string.IsNullOrEmpty(budgetId) ? string.Empty : budgetId;
        }

        public async Task<string> GetRememberedUserEmailAsync()
        {
            var email = await _secureStorage.GetAsync(RememberedUserEmail);
            return string.IsNullOrEmpty(email) ? string.Empty : email;
        }

        public async Task<string> GetRememberedUserPasswordAsync()
        {
            var password = await _secureStorage.GetAsync(RememberedUserPassword);
            return string.IsNullOrEmpty(password) ? string.Empty : password;
        }

        public async Task SetIsRememberedSwitchOnAsync(bool state)
        {
            await SecureStorage.Default.SetAsync(RememberedSwitchOn, state.ToString());
        }

        public async Task SetLastBudgetIdAsync(string id)
        {
            await SecureStorage.Default.SetAsync(LastBudgetId, id);
        }

        public async Task SetRememberedUserEmailAsync(string email)
        {
            await SecureStorage.Default.SetAsync(RememberedUserEmail, email);
        }

        public async Task SetRememberedUserPasswordAsync(string password)
        {
            await SecureStorage.Default.SetAsync(RememberedUserPassword, password);
        }
    }
}
