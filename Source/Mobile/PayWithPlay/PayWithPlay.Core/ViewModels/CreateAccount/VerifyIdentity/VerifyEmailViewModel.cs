using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity
{
    public class VerifyEmailViewModel : BaseVerifyIdentityViewModel
    {
        public override VerifyIdentity VerifyIdentityType => VerifyIdentity.Email;
        public override string Title => Resource.VerifyEmailTitle;
        public override string Subtitle => Resource.VerifyEmailSubtitle;
        public override string Message => Resource.VerifyEmailMessage;
    }
}