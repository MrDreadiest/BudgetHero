using BudgetHero.App.ViewModels.Details.Widgets;

namespace BudgetHero.App.Views.Details.Widgets;

public partial class ShortcutsDetailView : ContentPage
{
    private ShortcutsDetailViewModel _viewModel;
    public ShortcutsDetailView(ShortcutsDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}