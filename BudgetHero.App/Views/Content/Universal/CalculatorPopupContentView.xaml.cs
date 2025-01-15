using BudgetHero.App.ViewModels.Content.Universal;
using System.ComponentModel;

namespace BudgetHero.App.Views.Content.Universal;

public partial class CalculatorPopupContentView : ContentView
{
    private CalculatorPopupContentViewModel _viewModel;

    public CalculatorPopupContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is CalculatorPopupContentViewModel viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = viewModel;
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.IsVisible))
        {
            if (_viewModel.IsVisible)
            {
                bottomSheet.Show();
            }
            else
            {
                bottomSheet.Close();
            }
        }
    }
}