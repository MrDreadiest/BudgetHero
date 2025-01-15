using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Content.Widgets;
using System.ComponentModel;

namespace BudgetHero.App.Views.Content.Widgets;

public partial class ShortcutsContentView : ContentView
{

    private ShortcutsContentViewModel _viewModel;
    private FadeManager _fadeManager;

    public ShortcutsContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;

        _fadeManager = new FadeManager(this);
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is ShortcutsContentViewModel viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = viewModel;
            _fadeManager.ShowDuration = _viewModel.AnimationDuration;
            _fadeManager.HideDuration = _viewModel.AnimationDuration;
            _fadeManager.HideOpacity = 0;
            _fadeManager.HideOpacity = 0.9;
            _fadeManager.Initialize(_viewModel.IsOpen);
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.IsOpen))
        {
            await _fadeManager.HandleCollapse(_viewModel.IsOpen);
        }
    }
}