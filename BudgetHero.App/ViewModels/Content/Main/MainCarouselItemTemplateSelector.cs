namespace BudgetHero.App.ViewModels.Content.Main
{
    public class MainCarouselItemTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate LoginDT { get; set; }
        public required DataTemplate RegisterDT { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is LoginContentViewModel)
                return LoginDT;
            else if (item is RegisterContentViewModel)
                return RegisterDT;
            return null;
        }
    }
}
