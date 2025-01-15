using BudgetHero.App.Models.Shortcuts;
using BudgetHero.App.ViewModels.Content.Shortcuts;
using BudgetHero.App.ViewModels.Content.Widgets;

namespace BudgetHero.App.Factories.Shortcuts
{
    public static class ShortcutFactory
    {
        public static IShortcut CreateShortcut(ShortcutType shortcutType, ShortcutsContentViewModel shortcutsVM)
        {
            return new ShortcutContentViewModel(shortcutType.GetDescription(), shortcutType.GetIconUnicode(), shortcutType.GetDetailViewPath(), shortcutsVM);
        }
    }
}
