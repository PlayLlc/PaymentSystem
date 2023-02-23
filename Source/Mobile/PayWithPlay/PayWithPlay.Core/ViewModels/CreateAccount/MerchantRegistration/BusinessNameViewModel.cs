using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.MerchantRegistration
{
    public class BusinessNameViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<BusinessNameViewModel> _onContinueAction;
        private string? _businessName;

        public BusinessNameViewModel(Action<BusinessNameViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public string? BusinessName
        {
            get => _businessName;
            set => SetProperty(ref _businessName, value);
        }

        public string Title => Resource.BusinessNameTitle;
        public string BusinessNameText => Resource.BusinessName;
        public string ContinueText => Resource.Continue;

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }
    }
}