using BudgetHero.App.ViewModels.Details.Shortcuts;

namespace BudgetHero.App.Views.Details.Shortcuts;

public partial class AddTransactionDetailView : ContentPage
{
    private AddTransactionDetailViewModel _viewModel;
    public AddTransactionDetailView(AddTransactionDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void TotalAmount_ValueChanged(object sender, Syncfusion.Maui.Inputs.NumericEntryValueChangedEventArgs e)
    {
        if (e.NewValue is not null)
        {
            _viewModel.TemporaryTransaction.TotalAmount = (decimal)e.NewValue;
        }
        else
        {
            _viewModel.TemporaryTransaction.TotalAmount = 0;
        }
    }
}