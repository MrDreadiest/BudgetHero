using BudgetHero.App.ViewModels.Details.Widgets;

namespace BudgetHero.App.Views.Details.Widgets;

public partial class FastBalanceDetailView : ContentPage
{
    private FastBalanceDetailViewModel _viewModel;
    public FastBalanceDetailView(FastBalanceDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}