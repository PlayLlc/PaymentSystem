using AndroidX.AppCompat.Widget;
using Bumptech.Glide;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class ProfilePictureBinding : MvxAndroidTargetBinding<AppCompatImageView, string>
    {
        public const string Property = "ProfilePictureUrl";

        public ProfilePictureBinding(AppCompatImageView target) : base(target)
        {
        }

        protected override void SetValueImpl(AppCompatImageView target, string value)
        {
            if (target == null)
            {
                return;
            }

            Glide.With(target)
                .Load(value)
                .Placeholder(Resource.Drawable.ic_profile_picture_placeholder)
                .Into(target);
        }
    }
}
