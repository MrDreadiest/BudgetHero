using BudgetHero.App.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.Models
{
    [ResourceKey("Resource_Singular_Budget", "Resource_Plural_Budget")]
    public partial class Budget : ObservableObject
    {
        public required string Id { get; set; }
        public required string OwnerId { get; set; }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private string _iconUnicode = string.Empty;

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<TransactionCategory> TransactionCategories { get; set; } = new List<TransactionCategory>();
        public List<User> Users { get; private set; } = new List<User>();
    }
}
