using BudgetHero.App.ViewModels;

namespace BudgetHero.App.Views;

public partial class FirstSetupView : ContentPage
{
    FirstSetupViewModel _viewModel;
    public FirstSetupView(FirstSetupViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}