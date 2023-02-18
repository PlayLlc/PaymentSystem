using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Enums;

namespace PayWithPlay.Droid.CustomBindings
{
    public class VerifyIdentityImageBinding : MvxAndroidTargetBinding<AppCompatImageView, VerifyIdentityType>
    {
        public const string Property = "VerifyIdentityImageType";

        public VerifyIdentityImageBinding(AppCompatImageView target) : base(target)
        {
        }

        protected override void SetValueImpl(AppCompatImageView target, VerifyIdentityType value)
        {
            if(target == null)
            {
                return;
            }

            if (value == VerifyIdentityType.Email)
            {
                target.SetImageResource(Resource.Drawable.verify_email_image);
            }
            else
            {
                target.SetImageResource(Resource.Drawable.verify_phone_number_image);
            }
        }
    }
}