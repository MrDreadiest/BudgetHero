using BudgetHero.App.Factories.Confirmations;
using BudgetHero.App.Models.Configurations.Widgets;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Details.Widgets
{
    public partial class LastTransactionsDetailViewModel : WidgetDetailViewModelBase, IBusyHandler
    {
        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;


        [ObservableProperty]
        private int _count;

        private LastTransactionsConfiguration _configuration;

        private bool _isNavigatedTo;
        private bool _dataLoaded;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IConfigurationService _configurationService;

        public LastTransactionsDetailViewModel(IModalDisplayHandler displayHandler, IConfigurationService configurationService)
        {
            _displayHandler = displayHandler;
            _configurationService = configurationService;

            _configuration = new LastTransactionsConfiguration();

            Title = AppResource.Widget_LastTransactionsDetailView_Title;
        }

        [RelayCommand]
        private void NavigatedTo() => _isNavigatedTo = true;

        [RelayCommand]
        private void NavigatedFrom() => _isNavigatedTo = false;

        [RelayCommand]
        private async Task Appearing()
        {
            if (!_isNavigatedTo)
            {
                await Refresh();
            }
        }

        [RelayCommand]
        public override async Task Save()
        {
            var confirmation = ConfirmationFactory.CreateUpdateConfirmation<LastTransactionsConfiguration>();
            await this.RunWithBusyFlagAndConfirmationAsync(async () =>
            {
                SaveConfiguration();
                await Task.CompletedTask;
            }, confirmation);
        }

        public override void LoadConfiguration()
        {
            var configuration = _configurationService.LoadConfiguration<LastTransactionsConfiguration>();

            if (configuration != null)
            {
                _configuration = configuration;
                Count = _configuration.Count;
            }
        }

        public override void SaveConfiguration()
        {
            _configuration.Count = Count;
            _configurationService.SaveConfiguration(_configuration);
        }

        private async Task Refresh()
        {
            LoadConfiguration();
            await Task.CompletedTask;
        }
    }
}