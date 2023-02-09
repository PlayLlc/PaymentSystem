using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity
{
    public class VerifyPhoneNumberViewModel : BaseVerifyIdentityViewModel
    {
        public override VerifyIdentity VerifyIdentityType => VerifyIdentity.PhoneNumber;
        public override string Title => Resource.VerifyPhoneNumberTitle;
        public override string Subtitle => Resource.VerifyPhoneNumberSubtitle;
        public override string Message => Resource.VerifyPhoneNumberMessage;
    }
}