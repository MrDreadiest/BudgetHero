using BudgetHero.App.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Content.Budget
{
    public partial class BudgetListContentViewModel : ObservableObject
    {
        public BudgetListType BudgetListType { get; }

        public ObservableCollection<Models.Budget> Budgets { get; }

        private readonly IBudgetService _budgetService;

        public BudgetListContentViewModel(BudgetListType budgetListType) : this(App.Services.GetService<IBudgetService>()!, budgetListType)
        {
        }

        public BudgetListContentViewModel(IBudgetService budgetService, BudgetListType budgetListType)
        {
            _budgetService = budgetService;
            Budgets = new ObservableCollection<Models.Budget>();
            BudgetListType = budgetListType;
        }

        [RelayCommand]
        public void Select(Models.Budget budget)
        {
            _ = _budgetService.SelectBudgetAsCurrent(budget.Id);
        }

        public void Add(Models.Budget budget)
        {
            if (budget != null)
            {
                Budgets.Add(budget);
            }
        }

        public void Remove(Models.Budget budget)
        {
            var budgetToRemove = Budgets.ToList().Find(b => b.Id.Equals(budget.Id));

            if (budgetToRemove != null)
            {
                Budgets.Remove(budgetToRemove);
            }
        }

        public void Clear()
        {
            Budgets.Clear();
        }
    }
}
