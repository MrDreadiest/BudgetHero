using BudgetHero.App.ViewModels.Content.Transactions;
using Syncfusion.Maui.DataSource;

namespace BudgetHero.App.Views.Content.Widgets;

public partial class LastTransactionsContentView : ContentView
{
    public LastTransactionsContentView()
    {
        InitializeComponent();

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
}