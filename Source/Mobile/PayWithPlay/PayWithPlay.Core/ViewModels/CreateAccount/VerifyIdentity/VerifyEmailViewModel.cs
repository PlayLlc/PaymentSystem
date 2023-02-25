using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity
{
    public class VerifyEmailViewModel : BaseVerifyIdentityViewModel
    {
        public override VerifyIdentityType VerifyIdentityType => VerifyIdentityType.Email;
        public override TextStyleType TitleTextStyleType => TextStyleType.BigTitle;
        public override string Title => Resource.VerifyEmailTitle;
        public override string Subtitle => Resource.VerifyEmailSubtitle;
        public override string Message => Resource.VerifyEmailMessage;


        public override void OnVerify()
        {
            NavigationService.Navigate<EnableDeviceSettingsViewModel>();
        }
    }
}