using BudgetHero.App.ViewModels;

namespace BudgetHero.App.Views
{
    public partial class MainView : ContentPage
    {
        private MainViewModel _viewModel;
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}
