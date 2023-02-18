using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity
{
    public class VerifyPhoneNumberViewModel : BaseVerifyIdentityViewModel, ICreateAccountStep
    {
        private readonly Action<VerifyPhoneNumberViewModel> _onContinueAction;

        public VerifyPhoneNumberViewModel(Action<VerifyPhoneNumberViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public override VerifyIdentityType VerifyIdentityType => VerifyIdentityType.PhoneNumber;
        public override string Title => Resource.VerifyPhoneNumberTitle;
        public override string Subtitle => Resource.VerifyPhoneNumberSubtitle;
        public override string Message => Resource.VerifyPhoneNumberMessage;

        public override void OnVerify()
        {
            _onContinueAction?.Invoke(this);
        }
    }
}