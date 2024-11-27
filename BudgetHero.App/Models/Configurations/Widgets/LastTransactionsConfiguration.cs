using BudgetHero.App.Resources.Languages;
namespace BudgetHero.App.Models.Configurations.Widgets
{
    [ResourceKey("Resource_Singular_Configuration", "Resource_Singular_Configurations")]
    public class LastTransactionsConfiguration
    {
        private const int DefaultLastTransactionCount = 5;

        public int Count { get; set; }

        public LastTransactionsConfiguration()
        {
            Count = DefaultLastTransactionCount;
        }
    }
}
