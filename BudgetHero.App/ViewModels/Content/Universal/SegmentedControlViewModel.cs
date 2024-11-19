using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Universal
{
    public partial class SegmentedControlViewModel : ObservableObject
    {
        public event EventHandler<(int oldIndex, int newIndex)>? SelectionChanged;

        public List<SegmentedControlItem> Items { get; }

        [ObservableProperty]
        private SegmentedControlItem _selectedItem;

        private readonly bool _isSelectFromUiEnable;

        public SegmentedControlViewModel(List<SegmentedControlItem> items, bool isSelectFromUiEnable = true)
        {
            Items = items;
            SelectedItem = Items.First();

            _isSelectFromUiEnable = isSelectFromUiEnable;
        }

        public void Select(int index, bool notify = true)
        {
            Select(Items[index], notify);
        }

        [RelayCommand]
        public void Select(SegmentedControlItem item)
        {
            if (_isSelectFromUiEnable)
            {
                Select(item, true);
            }
        }

        private void Select(SegmentedControlItem? item, bool notify = true)
        {
            if (item != null)
            {
                int oldIndex = Items.IndexOf(SelectedItem);
                int newIndex = Items.IndexOf(item);

                if (oldIndex != newIndex)
                {
                    SelectedItem = item;

                    if (notify)
                    {
                        SelectionChanged?.Invoke(this, (oldIndex, newIndex));
                    }
                }
            }
        }

        partial void OnSelectedItemChanged(SegmentedControlItem? oldValue, SegmentedControlItem newValue)
        {
            int oldIndex = oldValue != null ? Items.IndexOf(oldValue) : 0;
            int newIndex = Items.IndexOf(newValue);

            if (oldValue != null)
            {
                oldValue.IsSelected = false;
            }
            newValue.IsSelected = true;

            SelectionChanged?.Invoke(this, (oldIndex, newIndex));
        }
    }

    public partial class SegmentedControlItem : ObservableObject
    {
        public string Text { get; }

        [ObservableProperty]
        private bool _isSelected;

        public SegmentedControlItem(string text)
        {
            Text = text;
        }
    }
}
