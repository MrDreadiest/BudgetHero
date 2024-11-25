namespace BudgetHero.App.Models.Filter
{
    public static class FilterGraphHelper
    {
        public static Dictionary<string, Dictionary<string, decimal>> GroupTransactionsByMonthAndCategory(List<Transaction> transactions, List<TransactionCategory> transactionCategories)
        {
            return transactions
                .GroupBy(t => t.Date.ToString("MMMM yyyy"))
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(t => transactionCategories.Find(c => c.Id.Equals(t.TransactionCategoryId))!.Id)
                          .ToDictionary(
                              c => c.Key,
                              c => c.Sum(t => t.TotalAmount)
                          )
                );
        }
    }
}
