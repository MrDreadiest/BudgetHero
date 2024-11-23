using BudgetHero.App.ViewModels.Content.Universal;

namespace BudgetHero.App.Views.Content.Universal;

public partial class FilterTransactionsContentView : ContentView
{
    FilterTransactionsContentViewModel _viewModel;

    public FilterTransactionsContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is FilterTransactionsContentViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }

    private void AmountFrom_ValueChanged(object sender, Syncfusion.Maui.Inputs.NumericEntryValueChangedEventArgs e)
    {
        if (e.NewValue is not null)
        {
            _viewModel.PriceFrom = (decimal)e.NewValue;
        }
        else
        {
            _viewModel.PriceFrom = 0;
        }
    }

    private void AmountTo_ValueChanged(object sender, Syncfusion.Maui.Inputs.NumericEntryValueChangedEventArgs e)
    {
        if (e.NewValue is not null)
        {
            _viewModel.PriceTo = (decimal)e.NewValue;
        }
        else
        {
            _viewModel.PriceTo = 0;
        }
    }
}