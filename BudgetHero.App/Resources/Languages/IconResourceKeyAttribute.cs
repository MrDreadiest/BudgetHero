namespace BudgetHero.App.Resources.Languages
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IconResourceKeyAttribute : Attribute
    {
        public string ResourceKey { get; }

        public IconResourceKeyAttribute(string resourceKey)
        {
            ResourceKey = resourceKey;
        }
    }
}
