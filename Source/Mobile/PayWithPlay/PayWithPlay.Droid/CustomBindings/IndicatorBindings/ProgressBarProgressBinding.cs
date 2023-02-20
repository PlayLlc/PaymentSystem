using Android.Animation;
using Android.OS;
using Android.Views.Animations;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings.IndicatorBindings
{
    public class ProgressBarProgressBinding : MvxAndroidTargetBinding<ProgressBar, int>
    {
        public const string Property = "ProgressBarProgress";

        public ProgressBarProgressBinding(ProgressBar target) : base(target)
        {
        }

        protected override void SetValueImpl(ProgressBar target, int value)
        {
            if (target == null)
            {
                return;
            }

            target.SetProgress(value, true);
        }
    }
}