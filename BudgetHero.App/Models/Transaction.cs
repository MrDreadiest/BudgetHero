using BudgetHero.App.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.Models
{
    [ResourceKey("Resource_Singular_Transaction", "Resource_Plural_Transaction")]
    public partial class Transaction : ObservableObject
    {
        public required string Id { get; set; }
        public required string BudgetId { get; set; }
        public required string TransactionCategoryId { get; set; }
        public required string CreatorId { get; set; }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private decimal _totalAmount;

        [ObservableProperty]
        private DateTime _date = DateTime.Now;

        public void Update(Transaction transaction)
        {
            transaction.Id = transaction.Id;
            transaction.BudgetId = transaction.BudgetId;
            transaction.TransactionCategoryId = transaction.TransactionCategoryId;
            transaction.CreatorId = transaction.CreatorId;
            transaction.Name = transaction.Name;
            transaction.Description = transaction.Description;
            transaction.TotalAmount = transaction.TotalAmount;
            transaction.Date = transaction.Date;
        }
    }
}
