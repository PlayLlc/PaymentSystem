using Android.Views;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.ViewModels.CreateAccount.MerchantRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Droid.CustomViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayWithPlay.Droid.CustomBindings.CreateAccountBindings
{
    public class BusinessNameViewBinding : MvxAndroidTargetBinding<View, BusinessNameViewModel>
    {
        public const string Property = "BusinessNameView";

        public BusinessNameViewBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, BusinessNameViewModel viewModel)
        {
            if (target == null || viewModel == null)
            {
                return;
            }

            Application.SynchronizationContext.Post(_ =>
            {
                var businessName = target.FindViewById<EditTextWithValidation>(Resource.Id.business_nameEt);
                viewModel.SetInputValidator(businessName);
            }, null);
        }
    }
}
