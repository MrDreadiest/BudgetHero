using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.Models.Icon
{
    public partial class IconItem : ObservableObject
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Unicode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [ObservableProperty]
        private bool _isSelected;
    }
}
