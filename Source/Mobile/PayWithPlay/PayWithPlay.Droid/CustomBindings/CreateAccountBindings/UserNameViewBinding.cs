using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.CustomBindings.CreateAccountBindings
{
    public class UserNameViewBinding : MvxAndroidTargetBinding<View, UserNameViewModel>
    {
        public const string Property = "UserNameView";

        public UserNameViewBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, UserNameViewModel viewModel)
        {
            if(target == null || viewModel == null)
            {
                return;
            }

            Application.SynchronizationContext.Post(_ =>
            {
                var firstName = target.FindViewById<EditTextWithValidation>(Resource.Id.first_nameEt);
                var lastName = target.FindViewById<EditTextWithValidation>(Resource.Id.last_nameEt);

                viewModel.SetInputValidators(firstName, lastName);
            }, null);
        }
    }
}
