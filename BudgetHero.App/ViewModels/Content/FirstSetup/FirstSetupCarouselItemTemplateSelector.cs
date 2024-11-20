namespace BudgetHero.App.ViewModels.Content.FirstSetup
{
    public class FirstSetupCarouselItemTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate UserDT { get; set; }
        public required DataTemplate BudgetDT { get; set; }
        public required DataTemplate CategoriesDT { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is UserContentViewModel)
                return UserDT;
            else if (item is BudgetContentViewModel)
                return BudgetDT;
            else if (item is CategoriesContentViewModel)
                return CategoriesDT;
            return null;
        }
    }
}
