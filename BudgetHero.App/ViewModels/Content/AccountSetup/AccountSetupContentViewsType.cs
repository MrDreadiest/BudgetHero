using BudgetHero.App.Models.Icon;
using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.ViewModels.Content.AccountSetup
{
    public enum AccountSetupContentViewsType
    {
        UserSetup,
        BudgetSetup,
        CategoriesSetup
    }

    public static class AccountSetupContentViewsTypeExtensions
    {
        public static string GetDescriptions(this AccountSetupContentViewsType viewsType)
        {
            return viewsType switch
            {
                AccountSetupContentViewsType.UserSetup => AppResource.AccountSetupContentViewsType_UserSetup,
                AccountSetupContentViewsType.BudgetSetup => AppResource.AccountSetupContentViewsType_BudgetSetup,
                AccountSetupContentViewsType.CategoriesSetup => AppResource.AccountSetupContentViewsType_CategoriesSetup,
                _ => string.Empty,
            };
        }
        public static string GetIcon(this AccountSetupContentViewsType viewsType)
        {
            return viewsType switch
            {
                AccountSetupContentViewsType.UserSetup => Icons.AccountCircle,
                AccountSetupContentViewsType.BudgetSetup => Icons.Budget,
                AccountSetupContentViewsType.CategoriesSetup => Icons.Categories,
                _ => string.Empty,
            };
        }
    }
}
