using BudgetHero.App.Models;

namespace BudgetHero.App.Services.Interfaces
{
    public interface IBudgetService
    {
        event EventHandler? CurrentBudgetChanged;
        event EventHandler? BudgetsChanged;

        event EventHandler<Budget>? BudgetCreated;
        event EventHandler<Budget>? BudgetDeleted;
        event EventHandler<Budget>? BudgetUpdated;

        IReadOnlyList<Budget> Budgets { get; }
        Budget CurrentBudget { get; set; }

        bool SelectBudgetAsCurrent(string id);
        Task TryGetLastBudgetAsCurrentAsync();

        Task<bool> GetAllBudgetsAsync();
        Task<Budget?> GetBudgetByIdAsync(string budgetId);
        Task<bool> CreateBudgetAsync(Budget budget);
        Task<bool> UpdateBudgetAsync(Budget budget);
        Task<bool> DeleteBudgetAsync(string budgetId);

    }
}
