using BudgetHero.App.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.Models
{
    [ResourceKey("Resource_Singular_Category", "Resource_Plural_Category")]
    public partial class TransactionCategory : ObservableObject
    {
        public required string Id { get; set; }
        public required string BudgetId { get; set; }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _iconUnicode = string.Empty;

        [ObservableProperty]
        private bool _isSelected;
    }
}
