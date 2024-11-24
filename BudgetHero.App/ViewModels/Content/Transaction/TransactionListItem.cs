using BudgetHero.App.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Content.Transactions
{
    public partial class TransactionListItem : ObservableObject
    {
        public Transaction Transaction { get; }
        public TransactionCategory TransactionCategory { get; }

        public TransactionListItem(Transaction transaction, TransactionCategory transactionCategory)
        {
            Transaction = transaction;
            TransactionCategory = transactionCategory;
        }
    }
}