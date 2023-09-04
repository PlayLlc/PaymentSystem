using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class PaymentManualEntryViewModel : BaseViewModel
    {
        private readonly IWheelPicker _wheelPicker;
        private readonly string[] _monthsValues = new[] { "January", "Febrary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private readonly string[] _yearsValues = new string[8];
        private readonly List<ITextInputValidator> _inputValidators = new();

        private string? _cardNumber;
        private string? _cardHolderName;
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
        public string CardHolderNameText => Resource.CardHolderName;
        public string ExpirationMonthText => Resource.ExpMonth;
        public string ExpirationYearText => Resource.ExpYear;
        public string CVVText => Resource.CVV;
        public string ScanCardButtonText => Resource.ScanCard;
        public string SubmitButtonText => Resource.Submit;

        public bool SubmitButtonEnabled => ValidationHelper.AreInputsValid(_inputValidators, false);

        public decimal TotalAmount { get; set; }

        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public Action? ScanCardAction { get; set; }

        public string? CardNumber
        {
            get => _cardNumber;
            set => SetProperty(ref _cardNumber, value, () => RaisePropertyChanged(() => SubmitButtonEnabled));
        }

        public string? CardHolderName
        {
            get => _cardHolderName;
            set => SetProperty(ref _cardHolderName, value, () => RaisePropertyChanged(() => SubmitButtonEnabled));
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
            ScanCardAction?.Invoke();
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

        public void OnSubmit() 
        {
            NavigationService.Navigate<ReceiptOptionsViewModel>();
        }

        public void SetInputValidators(ITextInputValidator cardNumber, ITextInputValidator cardHolderName, ITextInputValidator month, ITextInputValidator year, ITextInputValidator cvv)
        {
            ClearValidators();

            _inputValidators.Add(cardNumber);
            _inputValidators.Add(cardHolderName);
            _inputValidators.Add(month);
            _inputValidators.Add(year);
            _inputValidators.Add(cvv);

            cardNumber.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            cardHolderName.Validations = new List<IValidation>
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

        public void OnScanResult(string cardNumber, string cardHolderName, string expDate)
        {
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;

            if (expDate.Contains('/'))
            {
                var date = expDate.Split('/');
                if (int.TryParse(date[0], out int intMonth))
                {
                    Month = _monthsValues[intMonth - 1];
                }

                Year = _yearsValues.FirstOrDefault(t => t.Contains(date[1]));
            }
        }

        private void ClearValidators()
        {
            _inputValidators.Clear();
        }
    }
}
