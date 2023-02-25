using Android.Views;
using static Android.Views.View;

namespace PayWithPlay.Droid.Utils.Listeners
{
    public class NestedScrollViewOnTouchListener : Java.Lang.Object, IOnTouchListener
    {
        public bool OnTouch(View? v, MotionEvent? e)
        {
            if (e!.Action == MotionEventActions.Down)
            {
                v!.Parent!.RequestDisallowInterceptTouchEvent(true);
            }

            return false;
        }
    }
}