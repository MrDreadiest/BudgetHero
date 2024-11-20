using BudgetHero.App.ViewModels;

namespace BudgetHero.App.Views;

public partial class BudgetsView : ContentPage
{
    BudgetsViewModel _viewModel;
    public BudgetsView(BudgetsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}