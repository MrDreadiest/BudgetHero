using BudgetHero.App.ViewModels;
using BudgetHero.App.ViewModels.Content.Transactions;
using Syncfusion.Maui.DataSource;

namespace BudgetHero.App.Views;

public partial class TransactionsView : ContentPage
{
    TransactionsViewModel _viewModel;

    SearchBar _searchBar;

    public TransactionsView(TransactionsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        var groupDescriptor = new GroupDescriptor()
        {
            PropertyName = "Date",
            KeySelector = obj =>
            {
                var transaction = obj as TransactionListItem;
                return transaction?.Transaction.Date.Date;
            }
        };

        listView.DataSource!.GroupDescriptors.Add(groupDescriptor);
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _searchBar = (SearchBar)sender;
        if (listView.DataSource != null)
        {
            listView.DataSource.Filter = FilterItems;
            listView.DataSource.RefreshFilter();
        }
    }

    private bool FilterItems(object obj)
    {
        if (_searchBar == null || _searchBar?.Text == null)
        {
            return true;
        }
        var itemInfo = obj as TransactionListItem;


        if (itemInfo == null)
        {
            return true;
        }

        string searchText = _searchBar.Text.ToLower();

        if (itemInfo.Transaction.Name.ToLower().Contains(searchText) ||
            itemInfo.Transaction.Description.ToLower().Contains(searchText))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {
        _viewModel?.ToggleSwipeView(sender as SwipeView);
    }
}