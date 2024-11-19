using BudgetHero.App.Services.Interfaces;
using System.Text.Json;

namespace BudgetHero.App.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IBudgetService _budgetService;

        public ConfigurationService(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public T LoadConfiguration<T>() where T : class, new()
        {
            string configKey = GenerateConfigurationKey<T>();
            string configJson = Preferences.Get(configKey, string.Empty);
            return string.IsNullOrEmpty(configJson) ? new T() : JsonSerializer.Deserialize<T>(configJson);
        }

        public void SaveConfiguration<T>(T configuration)
        {
            string configKey = GenerateConfigurationKey<T>();
            string configJson = JsonSerializer.Serialize(configuration);

            Preferences.Set(configKey, configJson);
        }

        private string GenerateConfigurationKey<T>()
        {
            return string.Concat(_budgetService.CurrentBudget.Id, ":", nameof(T));
        }
    }
}
