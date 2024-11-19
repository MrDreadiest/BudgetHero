using BudgetHero.App.Services.Interfaces;

namespace BudgetHero.App.ViewModels.Interfaces
{
    public interface IBusyHandler
    {
        bool IsBusy { get; set; }
        IModalDisplayHandler? ModalDisplayHandler { get; }
    }
}
