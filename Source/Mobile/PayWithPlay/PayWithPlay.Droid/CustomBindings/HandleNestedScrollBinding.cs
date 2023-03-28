using Android.Views;
using AndroidX.Core.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Droid.Utils.Listeners;

namespace PayWithPlay.Droid.CustomBindings
{
    public class HandleNestedScrollBinding : MvxAndroidTargetBinding<View, bool>
    {
        public const string Property = "HanldeNestedScrolling";

        public HandleNestedScrollBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, bool value)
        {
            if (target == null)
            {
                return;
            }

            target.SetOnTouchListener(new NestedScrollViewOnTouchListener());
        }
    }
}
