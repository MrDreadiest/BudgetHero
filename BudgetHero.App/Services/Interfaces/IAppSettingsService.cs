namespace BudgetHero.App.Services.Interfaces
{
    public interface IAppSettingsService
    {
        Task<string> GetRememberedUserPasswordAsync();
        Task SetRememberedUserPasswordAsync(string password);
        Task DeleteRememberedUserPasswordAsync();

        Task<string> GetRememberedUserEmailAsync();
        Task SetRememberedUserEmailAsync(string email);
        Task DeleteRememberedUserEmailAsync();

        Task<string> GetLastBudgetIdAsync();
        Task SetLastBudgetIdAsync(string id);
        Task DeleteLastBudgetIdAsync();

        Task<bool> GetIsRememberedSwitchOnAsync();
        Task SetIsRememberedSwitchOnAsync(bool state);
        Task DeleteIsRememberedSwitchOnAsync();
    }
}
