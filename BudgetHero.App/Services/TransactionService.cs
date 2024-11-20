using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Transaction;

namespace BudgetHero.App.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IApiClient _apiClient;

        public event EventHandler<Transaction>? TransactionCreated;
        public event EventHandler<Transaction>? TransactionDeleted;
        public event EventHandler<Transaction>? TransactionUpdated;

        public TransactionService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> GetAllTransactionAsync(Budget budget)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budget.Id}" +
                $"{ApiEndpoints.Transaction}";

            var response = await _apiClient.GetAsync<List<TransactionGetResponseModel>>(url);
            if (response != null)
            {
                budget.Transactions.Clear();
                foreach (var transaction in response)
                {
                    budget.Transactions.Add(transaction.FromGetResponse());
                }
                return true;
            }
            return false;
        }

        public async Task<List<Transaction>> GetTransactionInRangeAsync(string budgetId, DateTime startDate, DateTime endDate)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budgetId}" +
                $"{ApiEndpoints.Transaction}" +
                $"{ApiEndpoints.DateRange}" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _apiClient.GetAsync<List<TransactionGetResponseModel>>(url);

            if (response != null)
            {
                return response.Select(r => r.FromGetResponse()).ToList();
            }
            return [];
        }

        public async Task<List<Transaction>> GetTransactionInRangeByCategoriesAsync(string budgetId, DateTime startDate, DateTime endDate, List<string> categoryIds)
        {
            string categoriesQuery = string.Join("&categoryIds=", categoryIds);
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budgetId}" +
                $"{ApiEndpoints.Transaction}" +
                $"{ApiEndpoints.Filtered}" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}" +
                $"&categoryIds={categoriesQuery}";

            var response = await _apiClient.GetAsync<List<TransactionGetResponseModel>>(url);

            if (response != null)
            {
                return response.Select(r => r.FromGetResponse()).ToList();
            }
            return [];
        }

        public async Task<Transaction?> GetTransactionByIdAsync(string budgetId, string transactionId)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budgetId}" +
                $"{ApiEndpoints.Transaction}" +
                $"/{transactionId}";

            var response = await _apiClient.GetAsync<TransactionGetResponseModel>(url);
            if (response != null)
            {
                return response.FromGetResponse();
            }
            return null;
        }

        public async Task<bool> CreateTransactionAsync(Budget budget, Transaction transaction)
        {
            return await CreateTransactionsAsync(budget, new List<Transaction>() { transaction });
        }

        public async Task<bool> CreateTransactionsAsync(Budget budget, List<Transaction> transactions)
        {
            string url = $"" +
                $"{ApiEndpoints.BaseAddress}" +
                $"{ApiEndpoints.Api}" +
                $"{ApiEndpoints.User}" +
                $"{ApiEndpoints.Budget}" +
                $"/{budget.Id}" +
                $"{ApiEndpoints.Transaction}";

            var response = await _apiClient.PostAsync<List<TransactionCreateRequestModel>, List<TransactionCreateResponseModel>>(url, transactions.Select(t => t.ToCreateRequest()).ToList());

            if (response != null)
            {
                foreach (var transaction in response)
                {
                    budget.Transactions.Add(transaction.FromCreateResponse());
                    TransactionCreated?.Invoke(this, transaction.FromCreateResponse());
                }
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateTransactionAsync(Budget budget, Transaction transaction)
        {
            var transactionToUpdate = budget.Transactions.Find(e => e.Id.Equals(transaction.Id));

            if (transactionToUpdate != null)
            {

                string url = $"" +
                    $"{ApiEndpoints.BaseAddress}" +
                    $"{ApiEndpoints.Api}" +
                    $"{ApiEndpoints.User}" +
                    $"{ApiEndpoints.Budget}" +
                    $"/{budget.Id}" +
                    $"{ApiEndpoints.Transaction}" +
                    $"/{transaction.Id}";

                var response = await _apiClient.PutAsync<TransactionUpdateRequestModel, TransactionUpdateResponseModel>(url, transaction.ToUpdateRequest());
                if (response != null)
                {
                    transactionToUpdate.FromUpdateResponse(response);
                    TransactionUpdated?.Invoke(this, transactionToUpdate);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteTransactionAsync(Budget budget, Transaction transaction)
        {
            var transactionToDelete = budget.Transactions.Find(e => e.Id.Equals(transaction.Id));

            if (transactionToDelete != null)
            {
                string url = $"" +
                    $"{ApiEndpoints.BaseAddress}" +
                    $"{ApiEndpoints.Api}" +
                    $"{ApiEndpoints.User}" +
                    $"{ApiEndpoints.Budget}" +
                    $"/{budget.Id}" +
                    $"{ApiEndpoints.Transaction}" +
                    $"/{transaction.Id}";

                var result = await _apiClient.DeleteAsync(url);
                if (result)
                {
                    budget.Transactions.Remove(transactionToDelete);
                    TransactionDeleted?.Invoke(this, transactionToDelete);
                    return true;
                }
            }
            return false;
        }
    }
}
