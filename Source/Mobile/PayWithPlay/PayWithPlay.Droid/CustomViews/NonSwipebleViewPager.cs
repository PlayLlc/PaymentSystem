using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.ViewPager.Widget;

namespace PayWithPlay.Droid.CustomViews
{
    public class NonSwipebleViewPager : ViewPager
    {
        public NonSwipebleViewPager(Context context) : base(context)
        {
        }

        public NonSwipebleViewPager(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        protected NonSwipebleViewPager(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public bool Swipeble { get; set; }

        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (Swipeble)
            {
                return base.OnInterceptTouchEvent(ev);
            }

            return false;
        }

        public override bool OnTouchEvent(MotionEvent? e)
        {
            if (Swipeble)
            {
                return base.OnTouchEvent(e);
            }

            return false;
        }
    }
}
