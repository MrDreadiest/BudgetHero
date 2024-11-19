using BudgetHero.App.Models.Icon;
using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.ViewModels.Content.FirstSetup
{
    public enum FirstSetupCarouselItemViewsType
    {
        UserSetup,
        BudgetSetup,
        CategoriesSetup
    }

    public static class FirstSetupCarouselItemViewsTypeExtensions
    {
        public static string GetDescriptions(this FirstSetupCarouselItemViewsType viewsType)
        {
            return viewsType switch
            {
                FirstSetupCarouselItemViewsType.UserSetup => AppResource.FirstSetupCarouselItemViewsType_UserSetup,
                FirstSetupCarouselItemViewsType.BudgetSetup => AppResource.FirstSetupCarouselItemViewsType_BudgetSetup,
                FirstSetupCarouselItemViewsType.CategoriesSetup => AppResource.FirstSetupCarouselItemViewsType_Categories,
                _ => string.Empty,
            };
        }
        public static string GetIcon(this FirstSetupCarouselItemViewsType viewsType)
        {
            return viewsType switch
            {
                FirstSetupCarouselItemViewsType.UserSetup => Icons.AccountCircle,
                FirstSetupCarouselItemViewsType.BudgetSetup => Icons.Budget,
                FirstSetupCarouselItemViewsType.CategoriesSetup => Icons.Categories,
                _ => string.Empty,
            };
        }
    }
}
