using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Universal
{
    public partial class AddBudgetContentViewModel : ObservableObject, IBusyHandler
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private Models.Budget _temporaryBudget;

        public IconSelectContentViewModel IconSelectVM { get; private set; }

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly IModalDisplayHandler _displayHandler;
        private readonly IBudgetService _budgetService;


        public AddBudgetContentViewModel() : this(
            App.Services.GetService<IModalDisplayHandler>()!,
            App.Services.GetService<IBudgetService>()!)
        {
        }

        public AddBudgetContentViewModel(IModalDisplayHandler displayHandler, IBudgetService budgetService)
        {
            _displayHandler = displayHandler;
            _budgetService = budgetService;

            Title = AppResource.AddBudgetContentView_Title;

            TemporaryBudget = new Models.Budget() { Id = string.Empty, OwnerId = string.Empty };

            IconSelectVM = new IconSelectContentViewModel();
            IconSelectVM.SelectedIconChanged += IconSelectVM_SelectedIconChanged;
        }

        [RelayCommand]
        public async Task Add()
        {
            var message = new Message("Powiadomienie", string.Empty);

            //TODO: Zasoby poprawić Exception handling
            await this.RunWithBusyFlagAsync(async () =>
            {
                if (TemporaryBudget.ToCreateRequest().IsCreateRequestValid())
                {
                    var result = await _budgetService.CreateBudgetAsync(TemporaryBudget);
                    if (result)
                    {
                        Refresh();
                        message.Body = "Akcja zakonczona sukcesem.";
                    }
                    else
                    {
                        message.Body = "Akcja zakonczona niepowodzeniem.";
                    }
                }
                else
                {
                    message.Body = "Parametry budżetu niepoprawne. Sprawdź wszystkie pola.";
                }
                IconSelectVM.ResetView();
            }, message);
        }

        public void Refresh()
        {
            IconSelectVM.ResetView();
            TemporaryBudget = new Models.Budget() { Id = string.Empty, OwnerId = string.Empty };
            TemporaryBudget.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
        }

        private void IconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (IconSelectVM.SelectedIconItem != null)
            {
                TemporaryBudget.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            }
        }
    }
}