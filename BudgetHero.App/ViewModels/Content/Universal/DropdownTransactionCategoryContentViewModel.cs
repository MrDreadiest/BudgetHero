using BudgetHero.App.Models;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Content.Universal
{
    public partial class DropdownTransactionCategoryContentViewModel : ObservableObject, IBusyHandler
    {
        public event EventHandler<List<TransactionCategory>>? SelectedTransactionCategoryChanged;

        [ObservableProperty]
        private TransactionCategory _lastSelectedTransactionCategory;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _filteredTransactionCategories = new();

        public ObservableCollection<TransactionCategory> SelectedCategories { get; private set; } = new();

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private bool _isEnable = true;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isAddMethodAvailable = false;

        [ObservableProperty]
        private bool _isMultipleSelectionAvailable = false;

        //public AddInlistCategoryContentViewModel AddInlistCategoryVM { get; set; }
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;



        public List<TransactionCategory> AllTransactionCategories = new();

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public DropdownTransactionCategoryContentViewModel() :
            this(App.Services.GetService<IModalDisplayHandler>()!, App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionCategoryService>()!)
        {

        }

        public DropdownTransactionCategoryContentViewModel(bool isAddMethodAvailable = false) :
    this(App.Services.GetService<IModalDisplayHandler>()!, App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionCategoryService>()!, false, isAddMethodAvailable)
        {

        }

        public DropdownTransactionCategoryContentViewModel(bool isMultipleSelectionAvailable, bool isAddMethodAvailable = false) :
            this(App.Services.GetService<IModalDisplayHandler>()!, App.Services.GetService<IBudgetService>()!, App.Services.GetService<ITransactionCategoryService>()!, isMultipleSelectionAvailable, isAddMethodAvailable)
        {

        }

        public DropdownTransactionCategoryContentViewModel(IModalDisplayHandler displayHandler, IBudgetService budgetService, ITransactionCategoryService transactionCategoryService, bool isMultipleSelectionAvailable = false, bool isAddMethodAvailable = false)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;
            _transactionCategoryService.TransactionCategoryCreated += TransactionCategoryService_TransactionCategoryCreated;

            IsMultipleSelectionAvailable = isMultipleSelectionAvailable;

            LastSelectedTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };

            IsAddMethodAvailable = isAddMethodAvailable;
            //if (IsAddMethodAvailable)
            //{
            //    AddInlistCategoryVM = new AddInlistCategoryContentViewModel();
            //}
        }


        [RelayCommand]
        private async Task Reload()
        {
            try
            {
                IsRefreshing = true;
                await ReloadDataAsync();
                UpdateSelectedObjects();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private void ToggleView()
        {
            IsVisible = !IsVisible;
        }

        [RelayCommand]
        private void Show()
        {
            IsVisible = true;
        }

        [RelayCommand]
        private void Hide()
        {
            IsVisible = false;
        }

        [RelayCommand]
        private void ItemTapped(TransactionCategory tappedCategory)
        {
            if (tappedCategory == null)
                return;

            if (IsMultipleSelectionAvailable)
            {
                tappedCategory.IsSelected = !tappedCategory.IsSelected;

                if (tappedCategory.IsSelected)
                {
                    SelectedCategories.Add(tappedCategory);
                }
                else
                {
                    SelectedCategories.Remove(tappedCategory);
                }
                LastSelectedTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
            }
            else
            {
                SelectedCategories.Clear();

                foreach (var category in FilteredTransactionCategories)
                {
                    category.IsSelected = false;
                }
                tappedCategory.IsSelected = true;

                SelectedCategories.Add(tappedCategory);
                LastSelectedTransactionCategory = SelectedCategories.Last();
            }
            SelectedTransactionCategoryChanged?.Invoke(this, SelectedCategories.ToList());
        }

        /// <summary>
        /// Select first n categories from top
        /// </summary>
        /// <param name="count"></param>
        public void SelectTopInRange(int count)
        {
            SelectCategories(AllTransactionCategories.Take(count).Select(c => c.Id));
        }

        /// <summary>
        /// Select categories by ids
        /// </summary>
        /// <param name="ids"></param>
        public void SelectCategories(IEnumerable<string> ids)
        {
            SelectedCategories.Clear();
            if (IsMultipleSelectionAvailable)
            {
                foreach (var id in ids)
                {
                    var category = AllTransactionCategories.FirstOrDefault(c => c.Id == id);
                    if (category != null)
                    {
                        category.IsSelected = true;
                        SelectedCategories.Add(category);
                    }
                }
            }
            else
            {
                var category = AllTransactionCategories.FirstOrDefault(c => c.Id == ids.First());
                if (category != null)
                {
                    category.IsSelected = true;
                    SelectedCategories.Add(category);
                }
            }
            SelectedTransactionCategoryChanged?.Invoke(this, SelectedCategories.ToList());
        }

        /// <summary>
        /// Reset viewModel to its origin state
        /// </summary>
        /// <returns></returns>
        public async Task Reset()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await ReloadDataAsync();
                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(AllTransactionCategories);
                SelectedCategories.Clear();

                LastSelectedTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };

                //AddInlistCategoryVM?.ResetView();
                SearchText = string.Empty;

                SelectedTransactionCategoryChanged?.Invoke(this, SelectedCategories.ToList());
            });
        }

        /// <summary>
        /// Must be fire on main view that contain this viewModel
        /// </summary>
        /// <returns></returns>
        public async Task Refresh()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                await ReloadDataAsync();
                UpdateSelectedObjects();

                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(AllTransactionCategories);
                SelectedCategories.Clear();

                //AddInlistCategoryVM?.ResetView();
                SearchText = string.Empty;

                SelectedTransactionCategoryChanged?.Invoke(this, SelectedCategories.ToList());
            });
        }

        private async Task ReloadDataAsync()
        {
            var result = await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);

            if (result)
            {
                AllTransactionCategories.Clear();
                foreach (var category in _budgetService.CurrentBudget.TransactionCategories)
                {
                    AllTransactionCategories.Add(category);
                }
            }

            FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(AllTransactionCategories);

            //AddInlistCategoryVM?.ResetView();
        }

        private void UpdateSelectedObjects()
        {

            var selectedCategories = new ObservableCollection<TransactionCategory>(SelectedCategories);
            if (selectedCategories.Count > 0)
            {
                SelectedCategories.Clear();
                foreach (var category in selectedCategories)
                {
                    var selectedCategory = FilteredTransactionCategories.FirstOrDefault(c => c.Id == category.Id);
                    if (selectedCategory != null)
                    {
                        selectedCategory.IsSelected = true;
                        SelectedCategories.Add(selectedCategory);
                    }
                }
                if (!IsMultipleSelectionAvailable)
                {
                    LastSelectedTransactionCategory = SelectedCategories.Last();
                }
                else
                {
                    LastSelectedTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
                }
                SelectedTransactionCategoryChanged?.Invoke(this, SelectedCategories.ToList());
            }
        }

        private void FilterTransactionCategories(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(AllTransactionCategories);
            }
            else
            {
                var filteredItems = AllTransactionCategories
                .Where(tc => tc.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(filteredItems);
            }
        }

        partial void OnSearchTextChanging(string? oldValue, string newValue)
        {
            FilterTransactionCategories(newValue);
        }

        //partial void OnSelectedCategoryChanged(TransactionCategory? oldValue, TransactionCategory newValue)
        //{
        //    if (oldValue != null)
        //    {
        //        oldValue.IsSelected = false;
        //    }

        //    newValue.IsSelected = true;

        //    SelectedTransactionCategoryChanged?.Invoke(newValue, new List<TransactionCategory> { newValue });
        //}

        private void TransactionCategoryService_TransactionCategoryCreated(object? sender, TransactionCategory e)
        {
            AllTransactionCategories.Add(e);
            FilteredTransactionCategories.Add(e);
        }
    }
}
