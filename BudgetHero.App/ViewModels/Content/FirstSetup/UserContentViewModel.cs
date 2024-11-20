using BudgetHero.App.Models;
using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetHero.App.ViewModels.Content.FirstSetup
{
    public partial class UserContentViewModel : FirstSetupCarouselItemViewModelBase, IBusyHandler
    {
        [ObservableProperty]
        private User _temporaryUser;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IModalDisplayHandler _displayHandler;

        public UserContentViewModel() : this(App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public UserContentViewModel(IModalDisplayHandler displayHandler) : base(FirstSetupCarouselItemViewsType.UserSetup)
        {
            _displayHandler = displayHandler;
            TemporaryUser = new User() { Id = string.Empty };
        }

        public override bool CanGoNext()
        {
            return TemporaryUser.ToUpdateRequest().IsUpdateRequestValid();
        }

        public override async Task OnAppearingAsync()
        {
            await Task.CompletedTask;
        }

        public override async Task OnDisappearingAsync()
        {
            await Task.CompletedTask;
        }

        public override async Task ResetViewAsync()
        {
            TemporaryUser = new User() { Id = string.Empty };
            await Task.CompletedTask;
        }
    }
}
