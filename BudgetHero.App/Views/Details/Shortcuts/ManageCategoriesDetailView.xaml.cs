using BudgetHero.App.ViewModels.Details.Shortcuts;

namespace BudgetHero.App.Views.Details.Shortcuts;

public partial class ManageCategoriesDetailView : ContentPage
{
    private ManageCategoriesDetailViewModel _viewModel;

    public ManageCategoriesDetailView(ManageCategoriesDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}