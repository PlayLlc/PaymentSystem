using AndroidX.Core.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Droid.Utils.Listeners;

namespace PayWithPlay.Droid.CustomBindings
{
    public class ScrollViewHandleNestedScrollBinding : MvxAndroidTargetBinding<NestedScrollView, bool>
    {
        public const string Property = "HanldeNestedScrolling";

        public ScrollViewHandleNestedScrollBinding(NestedScrollView target) : base(target)
        {
        }

        protected override void SetValueImpl(NestedScrollView target, bool value)
        {
            if (target == null)
            {
                return;
            }

            target.SetOnTouchListener(new NestedScrollViewOnTouchListener());
        }
    }
}
