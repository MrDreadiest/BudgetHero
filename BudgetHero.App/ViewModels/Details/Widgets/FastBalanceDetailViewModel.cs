﻿using BudgetHero.App.Factories.Confirmations;
using BudgetHero.App.Models.Configurations.Widgets;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Details.Widgets
{
    public partial class FastBalanceDetailViewModel : WidgetDetailViewModelBase, IBusyHandler
    {
        private readonly IModalDisplayHandler _displayHandler;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        public FastBalanceDetailViewModel(IModalDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;
            Title = AppResource.Widget_FastBalanceDetailView_Title;
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void SaveConfiguration()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public override async Task Save()
        {
            var confirmation = ConfirmationFactory.CreateUpdateConfirmation<FastBalanceConfiguration>();
            await this.RunWithBusyFlagAndConfirmationAsync(async () =>
            {
                await Task.Delay(2000);
            }, confirmation);
        }
    }
}