using BudgetHero.App.Models;

namespace BudgetHero.App.Services.Interfaces
{
    public interface ITransactionService
    {
        public event EventHandler<Transaction> TransactionCreated;
        public event EventHandler<Transaction> TransactionDeleted;
        public event EventHandler<Transaction> TransactionUpdated;

        Task<bool> GetAllTransactionAsync(Budget budget);
        Task<List<Transaction>> GetTransactionInRangeAsync(string budgetId, DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetTransactionInRangeByCategoriesAsync(string budgetId, DateTime startDate, DateTime endDate, List<string> categoryIds);
        Task<Transaction?> GetTransactionByIdAsync(string budgetId, string transactionId);
        Task<bool> CreateTransactionAsync(Budget budget, Transaction transaction);
        Task<bool> CreateTransactionsAsync(Budget budget, List<Transaction> transactions);
        Task<bool> UpdateTransactionAsync(Budget budget, Transaction transaction);
        Task<bool> DeleteTransactionAsync(Budget budget, Transaction transaction);
    }
}
