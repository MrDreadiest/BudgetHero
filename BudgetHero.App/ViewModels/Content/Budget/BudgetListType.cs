using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.ViewModels.Content.Budget
{
    public enum BudgetListType
    {
        Own,
        Shared
    }

    public static class BudgetListTypeExtensions
    {
        public static string GetDescription(this BudgetListType listType)
        {
            switch (listType)
            {
                case BudgetListType.Own:

                    return AppResource.BudgetListType_Own;
                case BudgetListType.Shared:
                    return AppResource.BudgetListType_Shared;
                default:
                    return string.Empty;
            }
        }
    }
}
