using BudgetHero.App.Models.Icon;
using BudgetHero.App.Views.Details.Shortcuts;

namespace BudgetHero.App.Models.Shortcuts
{
    public enum ShortcutType
    {
        SplitTransaction,
        AddTransaction,
        ManageCategories,
    }

    public static class ShortcutTypeExtensions
    {
        public static string GetDescription(this ShortcutType shortcutType)
        {
            switch (shortcutType)
            {
                case ShortcutType.AddTransaction:
                    return Resources.Languages.AppResource.Shortcut_AddTransactionDetailView_Title;
                case ShortcutType.SplitTransaction:
                    return Resources.Languages.AppResource.Shortcut_SplitTransactionDetailView_Title;
                case ShortcutType.ManageCategories:
                    return Resources.Languages.AppResource.Shortcut_ManageCategoriesView_Title;
                default:
                    return string.Empty;
            }
        }

        public static string GetIconUnicode(this ShortcutType shortcutType)
        {
            switch (shortcutType)
            {
                case ShortcutType.AddTransaction:
                    return Icons.TransactionAdd;
                case ShortcutType.SplitTransaction:
                    return Icons.TransactionSplit;
                case ShortcutType.ManageCategories:
                    return Icons.Categories;
                default:
                    return string.Empty;
            }
        }

        public static string GetDetailViewPath(this ShortcutType shortcutType)
        {
            switch (shortcutType)
            {
                case ShortcutType.AddTransaction:
                    return $"{nameof(AddTransactionDetailView)}";
                case ShortcutType.SplitTransaction:
                    return $"{nameof(SplitTransactionDetailView)}";
                case ShortcutType.ManageCategories:
                    return $"{nameof(ManageCategoriesDetailView)}";
                default:
                    return string.Empty;
            }
        }
    }
}
