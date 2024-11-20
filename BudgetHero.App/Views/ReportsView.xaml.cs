using BudgetHero.App.ViewModels;

namespace BudgetHero.App.Views;

public partial class ReportsView : ContentPage
{
    ReportsViewModel _viewModel;
    public ReportsView(ReportsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}