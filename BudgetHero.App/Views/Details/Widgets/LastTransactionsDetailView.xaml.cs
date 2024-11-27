using BudgetHero.App.ViewModels.Details.Widgets;

namespace BudgetHero.App.Views.Details.Widgets;

public partial class LastTransactionsDetailView : ContentPage
{
    private LastTransactionsDetailViewModel _viewModel;
    public LastTransactionsDetailView(LastTransactionsDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}