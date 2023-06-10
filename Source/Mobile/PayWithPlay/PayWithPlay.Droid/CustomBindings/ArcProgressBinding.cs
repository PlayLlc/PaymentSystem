using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.CustomBindings
{
    internal class ArcProgressBinding : MvxAndroidTargetBinding<ArcProgressView, float>
    {
        public const string Property = "ArcProgress";

        public ArcProgressBinding(ArcProgressView target) : base(target)
        {
        }

        protected override void SetValueImpl(ArcProgressView target, float value)
        {
            if (target == null) 
            {
                return;
            }

            target.SetProgress(value);
        }
    }
}
