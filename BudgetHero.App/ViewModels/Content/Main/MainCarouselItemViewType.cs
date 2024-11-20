namespace BudgetHero.App.ViewModels.Content.Main
{
    public enum MainCarouselItemViewType
    {
        Login,
        Register
    }

    public static class MainCarouselItemViewTypeExtensions
    {
        public static string GetDescription(this MainCarouselItemViewType viewsType)
        {
            switch (viewsType)
            {
                case MainCarouselItemViewType.Login:
                    return Resources.Languages.AppResource.MainCarouselItemViewType_Login;
                case MainCarouselItemViewType.Register:
                    return Resources.Languages.AppResource.MainCarouselItemViewType_Register;
                default:
                    return string.Empty;
            }
        }
    }
}
