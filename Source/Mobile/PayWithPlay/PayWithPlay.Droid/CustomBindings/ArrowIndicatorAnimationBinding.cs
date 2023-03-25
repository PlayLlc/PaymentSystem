using Android.Animation;
using Android.Views.Animations;
using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class ArrowIndicatorAnimationBinding : MvxAndroidTargetBinding<AppCompatImageView, bool>
    {
        public const string Property = "ArrowAnimation";

        public ArrowIndicatorAnimationBinding(AppCompatImageView target) : base(target)
        {
        }

        protected override void SetValueImpl(AppCompatImageView target, bool value)
        {
            if (target == null)
            {
                return;
            }
            var startAngle = 0f;
            var endAngle = 180f;

            if (!value)
            {
                startAngle = 180f;
                endAngle = 0f;
            }

            if (target.Rotation == endAngle)
            {
                return;
            }

            var animator = ObjectAnimator.OfFloat(target, "rotation", startAngle, endAngle);
            animator!.SetDuration(200);
            animator.SetAutoCancel(true);
            animator.SetInterpolator(new LinearInterpolator());
            animator.Start();
        }
    }
}