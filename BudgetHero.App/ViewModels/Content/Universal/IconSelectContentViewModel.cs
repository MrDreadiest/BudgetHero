using BudgetHero.App.Models.Icon;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
namespace BudgetHero.App.ViewModels.Content.Universal
{
    public partial class IconSelectContentViewModel : ObservableObject
    {
        public event EventHandler? SelectedIconChanged;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private IconItem _selectedIconItem;

        [ObservableProperty]
        private ObservableCollection<IconCategory> _iconCategories;

        public bool IsPopulated => IconCategories.Count > 0;

        public IconSelectContentViewModel()
        {
            IsVisible = false;
            IconCategories = new ObservableCollection<IconCategory>();
            SelectedIconItem = new IconItem() { Name = "DEFAULT", Unicode = Icons.Budget };
        }

        [RelayCommand]
        public void ToggleView()
        {
            IsVisible = !IsVisible;
        }

        [RelayCommand]
        public void SelectIcon(IconItem iconItem)
        {
            if (iconItem.IsSelected)
            {
                if (!SelectedIconItem.Name.Equals(iconItem.Name))
                {
                    iconItem.IsSelected = false;
                    SelectedIconItem = null;
                    return;
                }
            }

            if (SelectedIconItem != null)
            {
                SelectedIconItem.IsSelected = false;
            }

            iconItem.IsSelected = true;
            SelectedIconItem = iconItem;
        }

        partial void OnSelectedIconItemChanged(IconItem value)
        {
            SelectedIconChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ResetView()
        {
            IsVisible = false;

            if (IsPopulated)
            {
                SelectIcon(IconCategories.FirstOrDefault()!.Icons.FirstOrDefault()!);
            }
            else
            {
                ReloadData();
            }
        }

        private void ReloadData()
        {
            IconCategories.Clear();
            IconHelper.GetBudgetRelatedIcons()
            .Select(category => new IconCategory()
            {
                CategoryName = category.Key,
                Icons = category.Value
                .Select(icon => new IconItem()
                {
                    Name = icon.Name,
                    Unicode = icon.Unicode,
                    IsSelected = false
                }).ToList()
            }).ToList()
            .ForEach(category => { IconCategories.Add(category); });

            SelectIcon(IconCategories.FirstOrDefault()!.Icons.FirstOrDefault()!);
        }

    }
}
