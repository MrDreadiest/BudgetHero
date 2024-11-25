using BudgetHero.App.Models.Report;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;
using BudgetHero.App.ViewModels.Interfaces;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.ObjectModel;

namespace BudgetHero.App.ViewModels.Content.Reports
{
    public partial class ReportPieViewModel : ReportCarouselItemViewModelBase, IReport, IBusyHandler
    {
        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ObservableValue> ObservableValues { get; set; }

        public ReportType ReportType => ReportType.GraphPie;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        private readonly ReportViewModel _reportViewModel;
        private readonly IModalDisplayHandler _displayHandler;

        public ReportPieViewModel(ReportViewModel reportViewModel) : this(App.Services.GetService<IModalDisplayHandler>()!, reportViewModel)
        {
        }

        public ReportPieViewModel(IModalDisplayHandler modalDisplay, ReportViewModel reportViewModel)
        {
            _reportViewModel = reportViewModel;
            _displayHandler = modalDisplay;

            Title = ReportType.GetDescription();
            Series = new ObservableCollection<ISeries>();
            ObservableValues = new ObservableCollection<ObservableValue>();
        }

        /// <summary>
        /// It populates ObservableCollection Series.
        /// </summary>
        /// <returns></returns>
        public async Task DataPresentation()
        {
            await this.RunWithBusyFlagAsync(async () =>
            {
                PopulateSeries();
                await Task.CompletedTask;
            });
        }

        private void PopulateSeries()
        {
            var filteredData = _reportViewModel.FilteredData;
            var months = _reportViewModel.Months;
            var categories = _reportViewModel.Categories;
            var categoryColors = _reportViewModel.CategoryColors;

            Series.Clear();

            foreach (var category in categories)
            {
                var observableValues = new ObservableCollection<ObservableValue>();

                foreach (var month in months)
                {
                    if (filteredData[month].TryGetValue(category.Id, out decimal amount))
                    {
                        observableValues.Add(new ObservableValue((double)amount));
                    }
                    else
                    {
                        observableValues.Add(new ObservableValue(0));
                    }
                }

                Series.Add(new PieSeries<ObservableValue>
                {
                    Values = observableValues,
                    Name = category.Name,
                    MaxRadialColumnWidth = 65,
                    Fill = new SolidColorPaint(categoryColors[category.Id])
                });
            }
        }
    }
}
