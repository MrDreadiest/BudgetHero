using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetHero.App.ViewModels.Content.Universal
{
    public partial class CalculatorPopupContentViewModel : ObservableObject, IBusyHandler
    {
        public event EventHandler<decimal>? CalculationCompleted;

        public IModalDisplayHandler? ModalDisplayHandler => _displayHandler;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _formulaDisplay;

        [ObservableProperty]
        private string _resultDisplay;

        private readonly CalculatorModel _calculatorModel;

        private readonly IModalDisplayHandler _displayHandler;

        public CalculatorPopupContentViewModel() : this(App.Services.GetService<IModalDisplayHandler>()!)
        {
        }

        public CalculatorPopupContentViewModel(IModalDisplayHandler modalDisplayHandler)
        {
            _displayHandler = modalDisplayHandler;

            //TODO : Zasoby
            Title = "Kalkulator";

            _calculatorModel = new CalculatorModel();
            FormulaDisplay = _calculatorModel.CurrentFormula;
            ResultDisplay = "0";
        }

        [RelayCommand]
        public void ToggleView()
        {
            IsVisible = !IsVisible;
        }

        [RelayCommand]
        public void AppendToFormula(string value)
        {
            if (value == ",")
            {
                if (CanAddDecimalSeparator())
                {
                    _calculatorModel.CurrentFormula += ".";
                }
            }
            else
            {
                _calculatorModel.CurrentFormula += value;
            }

            Calculate();
            UpdateDisplays();
        }

        [RelayCommand]
        public void ClearFormula()
        {
            _calculatorModel.CurrentFormula = string.Empty;
            UpdateDisplays();
        }

        [RelayCommand]
        public void DeleteLastCharacter()
        {
            if (_calculatorModel.CurrentFormula.Length > 0)
            {
                _calculatorModel.CurrentFormula = _calculatorModel.CurrentFormula[..^1];
                Calculate();
                UpdateDisplays();
            }
        }

        [RelayCommand]
        public void Calculate()
        {
            try
            {
                var result = _calculatorModel.EvaluateFormula();
                ResultDisplay = result.ToString();
                CalculationCompleted?.Invoke(this, result);
            }
            catch
            {
                //TODO : Zasoby
                ResultDisplay = "Błąd!";
            }
        }

        [RelayCommand]
        public void Accept()
        {
            try
            {
                var result = _calculatorModel.EvaluateFormula();
                CalculationCompleted?.Invoke(this, result);
                FormulaDisplay = _calculatorModel.CurrentFormula;
                ResultDisplay = "0";
                _calculatorModel.CurrentFormula = string.Empty;
                ToggleView();
            }
            catch
            {
                //TODO : Zasoby
                ResultDisplay = "Błąd!";
                ToggleView();
            }
        }

        private void UpdateDisplays()
        {
            FormulaDisplay = _calculatorModel.CurrentFormula;
            ResultDisplay = string.IsNullOrEmpty(_calculatorModel.CurrentFormula) ? "0" : ResultDisplay;
        }

        private bool CanAddDecimalSeparator()
        {
            var lastNumber = GetLastNumber();
            return !lastNumber.Contains(".");
        }

        private string GetLastNumber()
        {
            var formulaParts = _calculatorModel.CurrentFormula.Split(new char[] { '+', '-', '*', '/' });
            return formulaParts.Length > 0 ? formulaParts[^1] : string.Empty;
        }
    }

    public class CalculatorModel
    {
        public string CurrentFormula { get; set; } = string.Empty;

        public decimal EvaluateFormula()
        {
            var dataTable = new System.Data.DataTable();
            return Convert.ToDecimal(dataTable.Compute(CurrentFormula, string.Empty));
        }
    }
}
