using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Content.Universal;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Content.FirstSetup
{
    public partial class BudgetContentViewModel : FirstSetupCarouselItemViewModelBase, IBusyHandler
    {
        [ObservableProperty]
        private Models.Budget _temporaryBudget;

        public IconSelectContentViewModel IconSelectVM { get; set; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IModalDisplayHandler _displayHandler;

        public BudgetContentViewModel(IconSelectContentViewModel iconSelectVM) : this(iconSelectVM, App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public BudgetContentViewModel(IconSelectContentViewModel iconSelectVM, IModalDisplayHandler displayHandler) : base(FirstSetupCarouselItemViewsType.BudgetSetup)
        {
            _displayHandler = displayHandler;

            TemporaryBudget = new Models.Budget() { Id = string.Empty, OwnerId = string.Empty };
            IconSelectVM = iconSelectVM;
        }

        public override bool CanGoNext()
        {
            return TemporaryBudget.ToCreateRequest().IsCreateRequestValid();
        }

        public override async Task OnAppearingAsync()
        {
            IconSelectVM.SelectedIconChanged += BudgetIconSelectVM_SelectedIconChanged;
            IconSelectVM.ResetView();
            await Task.CompletedTask;
        }

        public override async Task OnDisappearingAsync()
        {
            IconSelectVM.SelectedIconChanged -= BudgetIconSelectVM_SelectedIconChanged;
            await Task.CompletedTask;
        }

        public override async Task ResetViewAsync()
        {
            TemporaryBudget = new Models.Budget() { Id = string.Empty, OwnerId = string.Empty };
            TemporaryBudget.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            await Task.CompletedTask;
        }

        private void BudgetIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (IconSelectVM.SelectedIconItem != null)
            {
                TemporaryBudget.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            }
        }
    }
}
