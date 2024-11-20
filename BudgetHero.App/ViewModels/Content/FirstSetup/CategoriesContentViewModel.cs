using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Content.FirstSetup
{
    public partial class CategoriesContentViewModel : FirstSetupCarouselItemViewModelBase, IBusyHandler
    {

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _transactionCategories;

        [ObservableProperty]
        private TransactionCategory _temporaryTransactionCategory;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        public IconSelectContentViewModel IconSelectVM { get; set; }

        private readonly IModalDisplayHandler _displayHandler;

        public CategoriesContentViewModel(IconSelectContentViewModel iconSelectVM) : this(iconSelectVM, App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public CategoriesContentViewModel(IconSelectContentViewModel iconSelectVM, IModalDisplayHandler displayHandler) : base(FirstSetupCarouselItemViewsType.BudgetSetup)
        {
            _displayHandler = displayHandler;

            TransactionCategories = new ObservableCollection<TransactionCategory>();
            TemporaryTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
            IconSelectVM = iconSelectVM;
        }

        [RelayCommand]
        public void RemoveCategory(TransactionCategory transactionCategory)
        {
            TransactionCategories.Remove(transactionCategory);
        }

        [RelayCommand]
        public async Task AddCategory()
        {
            if (TemporaryTransactionCategory.ToCreateRequest().IsRequestValid())
            {
                TransactionCategories.Add(
                    new TransactionCategory()
                    {
                        Id = string.Empty,
                        BudgetId = string.Empty,
                        Name = TemporaryTransactionCategory.Name,
                        IconUnicode = TemporaryTransactionCategory.IconUnicode
                    }
                );
                IconSelectVM.ResetView();
                TemporaryTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
                TemporaryTransactionCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
                await Task.CompletedTask;
            }
        }

        public override bool CanGoNext()
        {
            int positive = TransactionCategories.Where(e => e.ToCreateRequest().IsRequestValid()).Count();
            return positive == TransactionCategories.Count();
        }

        public override async Task OnAppearingAsync()
        {
            IconSelectVM.SelectedIconChanged += TransactionCategoryIconSelectVM_SelectedIconChanged;
            IconSelectVM.ResetView();
            await Task.CompletedTask;
        }

        public override async Task OnDisappearingAsync()
        {
            IconSelectVM.SelectedIconChanged -= TransactionCategoryIconSelectVM_SelectedIconChanged;
            await Task.CompletedTask;
        }

        public override async Task ResetViewAsync()
        {
            TransactionCategories.Clear();

            TemporaryTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
            TemporaryTransactionCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;

            await Task.CompletedTask;
        }

        private void TransactionCategoryIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (IconSelectVM.SelectedIconItem != null)
            {
                TemporaryTransactionCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            }
        }
    }
}
