using BudgetHero.App.ViewModels.Details;

namespace BudgetHero.App.Views.Details;

public partial class TransactionDetailView : ContentPage
{
    TransactionDetailViewModel _viewModel;
    public TransactionDetailView(TransactionDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}