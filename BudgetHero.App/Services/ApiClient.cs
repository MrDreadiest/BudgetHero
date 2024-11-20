using BudgetHero.App.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BudgetHero.App.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public ApiClient(ITokenService tokenService)
        {
            _httpClient = new HttpClient();
            _tokenService = tokenService;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task PostAsync<T>(string endpoint, T data)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T2?> PostAsync<T1, T2>(string endpoint, T1 data)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T2>();
        }

        public async Task<T2?> PutAsync<T1, T2>(string endpoint, T1 data)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T2>();
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        private async Task AddAuthorizationHeader()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            var tokenType = await _tokenService.GetTokenTypeAsync();

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(tokenType))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
            }
        }


    }
}
