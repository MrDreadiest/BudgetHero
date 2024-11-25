using BudgetHero.App.ViewModels.Details;

namespace BudgetHero.App.Views.Details;

public partial class BudgetDetailView : ContentPage
{
    BudgetDetailViewModel _viewModel;
    public BudgetDetailView(BudgetDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}