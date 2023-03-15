using Android.Views;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.CustomBindings.CreateAccountBindings
{
    public class AddressViewBinding : MvxAndroidTargetBinding<View, RegistrationAddressViewModel>
    {
        public const string Property = "AddressView";

        public AddressViewBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, RegistrationAddressViewModel viewModel)
        {
            if (target == null || viewModel == null)
            {
                return;
            }

            Application.SynchronizationContext.Post(_ =>
            {
                var streetAddress = target.FindViewById<EditTextWithValidation>(Resource.Id.stree_addressEt);
                var apSuite = target.FindViewById<EditTextWithValidation>(Resource.Id.ap_suiteEt);
                var zipCode = target.FindViewById<EditTextWithValidation>(Resource.Id.zip_codeEt);
                var state = target.FindViewById<EditTextWithValidation>(Resource.Id.stateEt);
                var city = target.FindViewById<EditTextWithValidation>(Resource.Id.cityEt);

                viewModel.SetInputValidators(streetAddress, apSuite, zipCode, state, city);
            }, null);
        }
    }
}
