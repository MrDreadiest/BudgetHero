namespace BudgetHero.App.Models.Icon
{
    public class IconCategory
    {
        public string CategoryName { get; set; } = string.Empty;
        public List<IconItem> Icons { get; set; } = new();
    }
}
