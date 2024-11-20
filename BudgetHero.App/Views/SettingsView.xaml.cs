using BudgetHero.App.ViewModels;

namespace BudgetHero.App.Views;

public partial class SettingsView : ContentPage
{
    SettingsViewModel _viewModel;
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}