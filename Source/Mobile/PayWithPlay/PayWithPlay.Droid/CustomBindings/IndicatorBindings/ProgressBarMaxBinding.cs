using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings.IndicatorBindings
{
    public class ProgressBarMaxBinding : MvxAndroidTargetBinding<ProgressBar, int>
    {
        public const string Property = "ProgressBarMax";

        public ProgressBarMaxBinding(ProgressBar target) : base(target)
        {
        }

        protected override void SetValueImpl(ProgressBar target, int value)
        {
            if(target == null)
            {
                return;
            }

            target.Max = value;
        }
    }
}