using BudgetHero.App.ViewModels;

namespace BudgetHero.App.Views;

public partial class DashboardView : ContentPage
{
    DashboardViewModel _viewModel;
    public DashboardView(DashboardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}