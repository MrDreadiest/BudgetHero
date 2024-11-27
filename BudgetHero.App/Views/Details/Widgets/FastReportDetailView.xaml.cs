using BudgetHero.App.ViewModels.Details.Widgets;

namespace BudgetHero.App.Views.Details.Widgets;

public partial class FastReportDetailView : ContentPage
{
    private FastReportDetailViewModel _viewModel;
    public FastReportDetailView(FastReportDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}