using BudgetHero.App.ViewModels.Details.Shortcuts;

namespace BudgetHero.App.Views.Details.Shortcuts;

public partial class SplitTransactionDetailView : ContentPage
{
    private SplitTransactionDetailViewModel _viewModel;

    public SplitTransactionDetailView(SplitTransactionDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void AmountToSeparate_ValueChanged(object sender, Syncfusion.Maui.Inputs.NumericEntryValueChangedEventArgs e)
    {
        if (e.NewValue is not null)
        {
            _viewModel.AmountToSeparate = (decimal)e.NewValue;
        }
        else
        {
            _viewModel.AmountToSeparate = 0;
        }
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