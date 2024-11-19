using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.TransactionCategory;

namespace BudgetHero.App.Services
{
    public class TransactionCategoryService : ITransactionCategoryService
    {
        private readonly IApiClient _apiClient;

        public event EventHandler<TransactionCategory> TransactionCategoryCreated;
        public event EventHandler<TransactionCategory> TransactionCategoryDeleted;
        public event EventHandler<TransactionCategory> TransactionCategoryUpdated;

        public TransactionCategoryService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> GetAllTransactionCategoriesAsync(Budget budget)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budget.Id}" +
                $"{ApiEndpoints.TransactionCategory}";

            var response = await _apiClient.GetAsync<List<TransactionCategoryGetResponseModel>>(url);
            if (response != null)
            {
                budget.TransactionCategories.Clear();
                foreach (var item in response)
                {
                    budget.TransactionCategories.Add(item.FromGetResponse());
                }
                return true;
            }
            return false;
        }

        public async Task<List<TransactionCategory>> GetTopAmountTransactionCategoriesInDataRangeAsync(string budgetId, int count, DateTime startDate, DateTime endDate)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budgetId}" +
                $"{ApiEndpoints.TransactionCategory}" +
                $"{ApiEndpoints.TopAmount}" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}" +
                $"&count={count}";

            var response = await _apiClient.GetAsync<List<TransactionCategoryGetResponseModel>>(url);

            if (response != null)
            {
                return response.Select(r => r.FromGetResponse()).ToList();
            }
            return [];
        }

        public async Task<List<TransactionCategory>> GetTopCountTransactionCategoriesInDataRangeAsync(string budgetId, int count, DateTime startDate, DateTime endDate)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budgetId}" +
                $"{ApiEndpoints.TransactionCategory}" +
                $"{ApiEndpoints.TopCount}" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}" +
                $"&count={count}";

            var response = await _apiClient.GetAsync<List<TransactionCategoryGetResponseModel>>(url);

            if (response != null)
            {
                return response.Select(r => r.FromGetResponse()).ToList();
            }
            return [];
        }

        public Task<TransactionCategory> GetTransactionCategoryByIdsAsync(string budgetId, string categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateTransactionCategoriesAsync(Budget budget, List<TransactionCategory> transactionCategories)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budget.Id}" +
                $"{ApiEndpoints.TransactionCategory}";

            var response = await _apiClient.PostAsync<List<TransactionCategoryCreateRequestModel>, List<TransactionCategoryCreateResponseModel>>(url, transactionCategories.Select(e => e.ToCreateRequest()).ToList());
            if (response != null)
            {
                foreach (var item in response)
                {
                    budget.TransactionCategories.Add(item.FromCreateResponse());
                    TransactionCategoryCreated?.Invoke(this, item.FromCreateResponse());
                }
                return true;
            }
            return false;
        }

        public async Task<bool> CreateTransactionCategoryAsync(Budget budget, TransactionCategory transactionCategory)
        {
            return await CreateTransactionCategoriesAsync(budget, new List<TransactionCategory>() { transactionCategory });
        }

        public async Task<bool> UpdateTransactionCategoryAsync(Budget budget, TransactionCategory transactionCategory)
        {
            var categoryToUpdate = budget.TransactionCategories.Find(e => e.Id.Equals(transactionCategory.Id));

            if (categoryToUpdate != null)
            {
                string url = $"" +
                    $"{ApiEndpoints.BaseAddress}" +
                    $"{ApiEndpoints.Api}" +
                    $"{ApiEndpoints.User}" +
                    $"{ApiEndpoints.Budget}" +
                    $"/{transactionCategory.BudgetId}" +
                    $"{ApiEndpoints.TransactionCategory}" +
                    $"/{transactionCategory.Id}";

                var response = await _apiClient.PutAsync<TransactionCategoryUpdateRequestModel, TransactionCategoryUpdateResponseModel>(url, transactionCategory.ToUpdateRequest());
                if (response != null)
                {
                    categoryToUpdate.FromUpdateResponse(response);
                    TransactionCategoryUpdated?.Invoke(this, categoryToUpdate);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteTransactionCategoryAsync(Budget budget, TransactionCategory transactionCategory)
        {
            var categoryToDelete = budget.TransactionCategories.Find(e => e.Id.Equals(transactionCategory.Id));

            if (categoryToDelete != null)
            {
                string url = $"" +
                    $"{ApiEndpoints.BaseAddress}" +
                    $"{ApiEndpoints.Api}" +
                    $"{ApiEndpoints.User}" +
                    $"{ApiEndpoints.Budget}" +
                    $"/{budget.Id}" +
                    $"{ApiEndpoints.TransactionCategory}" +
                    $"/{transactionCategory.Id}";

                bool result = await _apiClient.DeleteAsync(url);

                if (result)
                {
                    budget.TransactionCategories.Remove(categoryToDelete);
                    TransactionCategoryDeleted?.Invoke(this, categoryToDelete);
                    return true;
                }
            }
            return false;
        }
    }
}
