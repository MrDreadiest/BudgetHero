using BudgetHero.App.ViewModels;

namespace BudgetHero.App.Views;

public partial class TransactionsView : ContentPage
{
    TransactionsViewModel _viewModel;
    public TransactionsView(TransactionsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}