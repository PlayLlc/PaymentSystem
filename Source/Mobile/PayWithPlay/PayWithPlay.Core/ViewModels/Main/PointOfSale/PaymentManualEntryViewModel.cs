using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;
using System.Drawing;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale
{
    public class PaymentManualEntryViewModel : BaseViewModel
    {
        private readonly IWheelPicker _wheelPicker;
        private readonly string[] _monthsValues = new[] { "January", "Febrary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private readonly string[] _yearsValues = new string[8]; 
        private readonly List<ITextInputValidator> _inputValidators = new();

        private string? _cardNubmer;
        private string? _month;
        private string? _year;
        private string? _cvv;

        public PaymentManualEntryViewModel(IWheelPicker wheelPicker)
        {
            _wheelPicker = wheelPicker;
            TotalAmount = 793.76m;

            var currentYear = DateTime.Now.Year;

            for (int i = 0; i < _yearsValues.Length; i++)
            {
                _yearsValues[i] = $"{currentYear + i}";
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            ClearValidators();
        }

        public string Title => Resource.ManualEntry;
        public string Subtitle => Resource.PaymentManualEntrySubtitle;
        public string CardNumberText => Resource.CardNumber;
        public string ExpirationMonthText => Resource.ExpirationMonth;
        public string ExpirationYearText => Resource.ExpirationYear;
        public string CVVText => Resource.CVV;
        public string ScanCardButtonText => Resource.ScanCard;
        public string SubmitButtonText => Resource.Submit;

        public bool SubmitButtonEnabled => ValidationHelper.AreInputsValid(_inputValidators, false);

        public decimal TotalAmount { get; set; }

        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public string? CardNumber
        {
            get => _cardNubmer;
            set => SetProperty(ref _cardNubmer, value, () => RaisePropertyChanged(() => SubmitButtonEnabled));
        }

        public string? Month
        {
            get => _month;
            set => SetProperty(ref _month, value, () => RaisePropertyChanged(() => SubmitButtonEnabled));
        }

        public string? Year
        {
            get => _year;
            set => SetProperty(ref _year, value, () => RaisePropertyChanged(() => SubmitButtonEnabled));
        }

        public string? CVV
        {
            get => _cvv;
            set => SetProperty(ref _cvv, value, () => RaisePropertyChanged(() => SubmitButtonEnabled));
        }

        public void OnScanCard()
        {
        }

        public void OnMonth()
        {
            var index = Array.IndexOf(_monthsValues, Month);

            _wheelPicker.Show(_monthsValues, index == -1 ? 0 : index, "Select expiration month", Resource.Ok, Resource.Cancel, (index) =>
            {
                Month = _monthsValues[index];
            });
        }

        public void OnYear() 
        {
            var index = Array.IndexOf(_yearsValues, Year);

            _wheelPicker.Show(_yearsValues, index == -1 ? 0 : index, "Select expiration year", Resource.Ok, Resource.Cancel, (index) =>
            {
                Year = _yearsValues[index];
            });
        }

        public void OnSubmit() { }

        public void SetInputValidators(ITextInputValidator cardNumber, ITextInputValidator month, ITextInputValidator year, ITextInputValidator cvv)
        {
            ClearValidators();

            _inputValidators.Add(cardNumber);
            _inputValidators.Add(month);
            _inputValidators.Add(year);
            _inputValidators.Add(cvv);

            cardNumber.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            month.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            year.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            cvv.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };

            RaisePropertyChanged(() => SubmitButtonEnabled);
        }

        private void ClearValidators()
        {
            _inputValidators.Clear();
        }
    }
}
