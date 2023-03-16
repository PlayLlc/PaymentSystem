using Android.Views;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.CustomBindings.CreateAccountBindings
{
    public class PhoneNumberViewBinding : MvxAndroidTargetBinding<View, UserPhoneNumberViewModel>
    {
        public const string Property = "PhoneNumberView";

        public PhoneNumberViewBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, UserPhoneNumberViewModel viewModel)
        {
            if (target == null || viewModel == null)
            {
                return;
            }

            Application.SynchronizationContext.Post(_ =>
            {
                var phoneNumber = target.FindViewById<EditTextWithValidation>(Resource.Id.phone_numberEt);
                viewModel.SetInputValidator(phoneNumber);
            }, null);
        }
    }
}
