namespace BudgetHero.App.Services.Interfaces
{
    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task PostAsync<T>(string endpoint, T data);
        Task<T2?> PostAsync<T1, T2>(string endpoint, T1 data);
        Task<T2?> PutAsync<T1, T2>(string endpoint, T1 data);
        Task<bool> DeleteAsync(string endpoint);
    }
}
