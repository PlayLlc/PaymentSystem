using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class HomeOrBusinessAddressViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<HomeOrBusinessAddressViewModel>? _onContinueAction;

        public HomeOrBusinessAddressViewModel(Action<HomeOrBusinessAddressViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public string Title => Resource.HomeOrBusinessAddressQuestion;
        public string YesText => Resource.Yes;
        public string NoText => Resource.No;

        public bool SameAddress { get; private set; }

        public void OnYes()
        {
            SameAddress = true;
            _onContinueAction?.Invoke(this);
        }

        public void OnNo() 
        {
            SameAddress = false;
            _onContinueAction?.Invoke(this);
        }
    }
}