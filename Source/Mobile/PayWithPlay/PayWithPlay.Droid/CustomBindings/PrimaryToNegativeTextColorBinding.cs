using Android.Graphics;
using AndroidX.Core.Content;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class PrimaryToNegativeTextColorBinding : MvxAndroidTargetBinding<TextView, bool>
    {
        public const string Property = "PrimaryToNegativeTextColor";

        public PrimaryToNegativeTextColorBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, bool value)
        {
            if (target == null)
            {
                return;
            }

            if (value)
            {
                target.SetTextColor(new Color(ContextCompat.GetColor(target.Context, Resource.Color.negative_color)));
            }
            else
            {
                target.SetTextColor(new Color(ContextCompat.GetColor(target.Context, Resource.Color.primary_text_color)));
            }
        }
    }
}
