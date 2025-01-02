using BudgetHero.App.Models.Shortcuts;
using BudgetHero.App.ViewModels.Content.Shortcuts;

namespace BudgetHero.App.Factories.Shortcuts
{
    public static class ShortcutFactory
    {
        public static IShortcut CreateShortcut(ShortcutType shortcutType)
        {
            return new ShortcutContentViewModel(shortcutType.GetDescription(), shortcutType.GetIconUnicode(), shortcutType.GetDetailViewPath());
        }
    }
}
