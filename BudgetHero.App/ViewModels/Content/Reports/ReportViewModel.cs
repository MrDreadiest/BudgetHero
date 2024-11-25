using BudgetHero.App.Models;
using BudgetHero.App.Models.Filter;
using SkiaSharp;

namespace BudgetHero.App.ViewModels.Content.Reports
{
    public class ReportViewModel
    {
        public event EventHandler<bool>? IsPercentageVisibleChanged;
        public event EventHandler<bool>? IsCategoryNameVisibleChanged;

        public Dictionary<string, Dictionary<string, decimal>> FilteredData { get; set; }
        public Dictionary<string, SKColor> CategoryColors { get; set; }
        public List<string> Months { get; set; }
        public List<TransactionCategory> Categories { get; set; }

        public decimal AllTransactionsAmount { get; set; }
        public decimal AvgTransactionsAmount { get; set; }

        public bool IsPercentageVisible => _isPercentageVisible;
        public bool IsCategoryNameVisible => _isCategoryNameVisible;

        private bool _isPercentageVisible = false;
        private bool _isCategoryNameVisible = true;


        public ReportViewModel()
        {
            FilteredData = new();
            CategoryColors = new();
            Months = new();
            Categories = new();
        }

        /// <summary>
        /// Set is percentage label is shown on table report
        /// </summary>
        /// <param name="isVisible"></param>
        public void SetPercentageVisible(bool isVisible)
        {
            _isPercentageVisible = isVisible;
            IsPercentageVisibleChanged?.Invoke(this, isVisible);
        }

        /// <summary>
        /// Set is category name is visible
        /// </summary>
        /// <param name="isVisible"></param>
        public void SetCategoryNameVisible(bool isVisible)
        {
            _isCategoryNameVisible = isVisible;
            IsCategoryNameVisibleChanged?.Invoke(this, isVisible);
        }

        public void UpdateData(List<Transaction> transactions, List<TransactionCategory> categories)
        {
            FilteredData = FilterGraphHelper.GroupTransactionsByMonthAndCategory(transactions, categories);
            Categories = categories;

            Months = FilteredData.Keys.OrderBy(m => DateTime.ParseExact(m, "MMMM yyyy", null)).ToList();

            AllTransactionsAmount = transactions.Sum(transaction => transaction.TotalAmount);
            AvgTransactionsAmount = AllTransactionsAmount / Months.Count;
        }

        public void GenerateCategoryColors()
        {
            Random random = new Random();
            float hueStep = 360f / Categories.Count;
            int saturation = 70;
            int value = 90;

            foreach (var category in Categories)
            {
                float hue = (hueStep * Categories.IndexOf(category)) % 360;
                var color = SKColor.FromHsv(hue, saturation, value);
                CategoryColors[category.Id] = color;
            }
        }
    }
}
