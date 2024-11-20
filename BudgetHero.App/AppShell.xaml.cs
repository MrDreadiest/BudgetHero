using BudgetHero.App.Models.Extensions;
using BudgetHero.App.Models.Icon;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.Views;

namespace BudgetHero.App
{
    public partial class AppShell : Shell
    {
        private readonly IBudgetService _budgetService;
        private readonly IUserService _userService;

        public AppShell() : this(App.Services.GetService<IBudgetService>()!, App.Services.GetService<IUserService>()!)
        {

        }

        public AppShell(IBudgetService budgetService, IUserService userService)
        {
            Resources.Add("DashboardIcon", Icons.Home);
            Resources.Add("TransactionsIcon", Icons.Transactions);
            Resources.Add("ReportsIcon", Icons.Graphs);
            Resources.Add("BudgetIcon", Icons.Budget);
            Resources.Add("SettingsIcon", Icons.Settings);

            InitializeComponent();

            _budgetService = budgetService;
            //_budgetService.CurrentBudgetChanged += _budgetService_CurrentBudgetChanged;
            _userService = userService;

            Navigating += OnShellNavigating;

            Application.Current!.UserAppTheme = AppTheme.Dark;

            //var currentTheme = Application.Current!.UserAppTheme;
            //ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;
        }

        //private void _budgetService_CurrentBudgetChanged(object? sender, EventArgs e)
        //{
        //    DashboardView.IsEnabled = !_budgetService.CurrentBudget.IsNullOrEmpty();
        //}

        private void OnShellNavigating(object? sender, ShellNavigatingEventArgs e)
        {
            if (e.Target.Location.OriginalString.Contains(nameof(DashboardView)))
            {
                if (!_userService.CurrentUser.IsAccountSetup)
                {
                    e.Cancel();
                    GoToAsync($"//{nameof(FirstSetupView)}").FireAndForgetSafeAsync();
                }
                else if (_budgetService.CurrentBudget.IsNullOrEmpty())
                {
                    e.Cancel();
                    GoToAsync($"//{nameof(BudgetsView)}").FireAndForgetSafeAsync();
                }
            }
        }

        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }
    }
}
