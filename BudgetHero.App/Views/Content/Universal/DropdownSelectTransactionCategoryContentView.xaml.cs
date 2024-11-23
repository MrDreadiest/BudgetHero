using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Universal;
using System.ComponentModel;

namespace BudgetHero.App.Views.Content.Universal;

public partial class DropdownSelectTransactionCategoryContentView : ContentView
{
    private DropdownTransactionCategoryContentViewModel _viewModel;
    private FadeManager _fadeManager;

    public DropdownSelectTransactionCategoryContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;

        _fadeManager = new FadeManager(this);
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is DropdownTransactionCategoryContentViewModel viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = viewModel;
            _fadeManager.Initialize(_viewModel.IsVisible);
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.IsVisible))
        {
            await _fadeManager.HandleCollapse(_viewModel.IsVisible);
        }
    }
}