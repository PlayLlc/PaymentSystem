using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Bumptech.Glide;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class GlideImageBinding : MvxAndroidTargetBinding<AppCompatImageView, string>
    {
        public const string Property = "ImageUrl";

        public GlideImageBinding(AppCompatImageView target) : base(target)
        {
        }

        protected override void SetValueImpl(AppCompatImageView target, string value)
        {
            if(target == null)
            {
                return;
            }

            var placeholderDrawable = ContextCompat.GetDrawable(target.Context, Resource.Drawable.ic_image_placeholder)!;
            placeholderDrawable.SetTint(ContextCompat.GetColor(target.Context, Resource.Color.accent_color));

            Glide.With(target)
                .Load(value)
                .Placeholder(placeholderDrawable)
                .Into(target);
        }
    }
}
