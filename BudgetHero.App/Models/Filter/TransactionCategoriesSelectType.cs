using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.Models.Filter
{
    public enum TransactionCategoriesSelectType
    {
        Own,
        TopCount,
        TopAmount
    }

    public static class TransactionCategoriesSelectTypeExtension
    {
        public static string GetDescription(this TransactionCategoriesSelectType selectType)
        {
            switch (selectType)
            {
                case TransactionCategoriesSelectType.Own:
                    return AppResource.TransactionCategoriesSelectType_Own;
                case TransactionCategoriesSelectType.TopCount:
                    return AppResource.TransactionCategoriesSelectType_TopCount;
                case TransactionCategoriesSelectType.TopAmount:
                    return AppResource.TransactionCategoriesSelectType_TopAmount;
                default:
                    return string.Empty;
            }
        }
    }
}
