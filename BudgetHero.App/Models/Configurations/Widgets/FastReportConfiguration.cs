using BudgetHero.App.Models.Filter;
using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.Models.Configurations.Widgets
{
    [ResourceKey("Resource_Singular_Configuration", "Resource_Singular_Configurations")]
    public class FastReportConfiguration
    {
        public const int DefaultTopTransactionCount = 3;
        public const int TopCounterMin = 3;

        public FastReportConfiguration()
        {
            SelectType = TransactionCategoriesSelectType.TopAmount.ToString();
            SelectedCategoriesIds = new List<string>();
            TopCounter = DefaultTopTransactionCount;
            IsShowPercentageSwitch = true;
            IsSumOtherAsLastSwitch = true;
        }

        public string SelectType { get; set; } = TransactionCategoriesSelectType.Own.ToString();
        public List<string> SelectedCategoriesIds { get; set; }
        public int TopCounter { get; set; }
        public bool IsShowPercentageSwitch { get; set; }
        public bool IsSumOtherAsLastSwitch { get; set; }
    }
}
